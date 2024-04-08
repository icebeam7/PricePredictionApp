using PricePredictionApp.Models;

namespace PricePredictionApp.Services
{
    public interface IDataService
    {
        Task<IEnumerable<TaxiTrip>> GetData();
        Task<List<TaxiTrip>> ReadCsvFile(string csvFile, int count = 50);
    }
}
