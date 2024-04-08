using Microsoft.ML.Data;

namespace PricePredictionApp.Models
{
    public class TaxiTrip
    {
        [LoadColumn(0)]
            [ColumnName(@"vendor_id")]
        public string? VendorId;

            [ColumnName(@"rate_code")]
        [LoadColumn(1)]
        public string? RateCode;

            [ColumnName(@"passenger_count")]
        [LoadColumn(2)]
        public float PassengerCount;

            [ColumnName(@"trip_time_in_secs")]
        [LoadColumn(3)]
        public float TripTime;

            [ColumnName(@"trip_distance")]
        [LoadColumn(4)]
        public float TripDistance;

            [ColumnName(@"payment_type")]
        [LoadColumn(5)]
        public string? PaymentType;

        [LoadColumn(6)]
            [ColumnName(@"fare_amount")]
        public float FareAmount;
    }
}
