using PricePredictionApp.Models;

namespace PricePredictionApp.Services
{
    public class DataService : IDataService
    {
        string trainDataPath = "taxi-fare-train.csv";
        string testDataPath = "taxi-fare-test.csv";

        public async Task<IEnumerable<TaxiTrip>> GetData()
        {
            var trainData = await ReadCsvFile(trainDataPath);
            var testData = await ReadCsvFile(testDataPath);

            return trainData.Union(testData);
        }

        public async Task<List<TaxiTrip>> ReadCsvFile(string csvFile, int count = 50)
        {
            var list = new List<TaxiTrip>();

            using var stream = await FileSystem.OpenAppPackageFileAsync(csvFile);
            using var reader = new StreamReader(stream);

            var data = await reader.ReadToEndAsync();
            var rows = data.Split("\n").ToList();

            //for (int i = 0; i < rows.Count; i++)
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    var row = rows[i].Split(",").ToList();

                    var taxiTrip = new TaxiTrip()
                    {
                        VendorId = row[0],
                        RateCode = row[1],
                        PassengerCount = float.Parse(row[2]),
                        TripTime = float.Parse(row[3]),
                        TripDistance = float.Parse(row[4]),
                        PaymentType = row[5],
                        FareAmount = float.Parse(row[6])
                    };

                    list.Add(taxiTrip);
                }
            }

            return list;
        }
    }
}
