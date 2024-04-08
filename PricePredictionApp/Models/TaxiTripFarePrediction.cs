using Microsoft.ML.Data;

namespace PricePredictionApp.Models
{
    public class TaxiTripFarePrediction
    {
        [ColumnName("Score")]
        public float FareAmount;
    }
}
