using Microsoft.ML;
using Microsoft.ML.Data;
using PricePredictionApp.Models;

namespace PricePredictionApp.Services
{
    public interface IMLService
    {
        static string dataDir;
        Task Train();
        Task<RegressionMetrics> Evaluate();
        TaxiTripFarePrediction TestSinglePrediction(TaxiTrip taxiTrip);
        ITransformer LoadModel();
        void SaveModel(ITransformer model, IDataView data);



    }
}
