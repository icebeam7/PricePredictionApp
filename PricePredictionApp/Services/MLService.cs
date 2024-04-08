using Microsoft.ML;
using Microsoft.ML.Data;
using PricePredictionApp.Models;

namespace PricePredictionApp.Services
{
    public class MLService : IMLService
    {
        string trainDataPath = "taxi-fare-train.csv";
        string testDataPath = "taxi-fare-test.csv";

        string modelPath = Path.Combine(FileSystem.AppDataDirectory, "Model.zip");

        MLContext mlContext;
        ITransformer model;

        IDataService dataService;

        public MLService(IDataService dataService, int seed = 0)
        {
            this.dataService = dataService;

            mlContext = new MLContext(seed);
        }

        public async Task Train()
        {
            var data = await dataService.ReadCsvFile(trainDataPath);

            var dataView = mlContext.Data.LoadFromEnumerable<TaxiTrip>(data);
            var pipeline = mlContext.Transforms
                .CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(
                    outputColumnName: "VendorIdEncoded", inputColumnName: "VendorId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(
                    outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(
                    outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
                .Append(mlContext.Transforms.Concatenate(
                    "Features", "VendorIdEncoded", "RateCodeEncoded", 
                    "PassengerCount", "TripDistance", "PaymentTypeEncoded"))
                .Append(mlContext.Regression.Trainers.FastTree());

            model = pipeline.Fit(dataView);
            SaveModel(model, dataView);
        }

        public async Task<RegressionMetrics> Evaluate()
        {
            if (model == null)
                model = LoadModel();

            var data = await dataService.ReadCsvFile(testDataPath);
            var dataView = mlContext.Data.LoadFromEnumerable<TaxiTrip>(data);

            var predictions = model.Transform(dataView);

            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
            return metrics;
        }

        public TaxiTripFarePrediction TestSinglePrediction(TaxiTrip taxiTrip)
        {
            if (model == null)
                model = LoadModel();

            var predictionFunction = mlContext.Model.CreatePredictionEngine<TaxiTrip, TaxiTripFarePrediction>(model);

            var prediction = predictionFunction.Predict(taxiTrip);
            return prediction;
        }

        public ITransformer LoadModel()
        {
            DataViewSchema modelSchema;
            ITransformer model = mlContext.Model.Load(modelPath, out modelSchema);
            return model;
        }

        public void SaveModel(ITransformer model, IDataView data)
        {
            mlContext.Model.Save(model, data.Schema, modelPath);
        }
    }
}
