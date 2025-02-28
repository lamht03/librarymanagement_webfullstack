using System;
using System.Text.Json.Serialization;

namespace LibraryManagement.Models
{
    public class Payment
    {
        [JsonPropertyName("PaymentID")]
        public int PaymentID { get; set; }

        [JsonPropertyName("TransactionID")]
        public int TransactionID { get; set; }

        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("PaymentDate")]
        public DateTime PaymentDate { get; set; }

        [JsonPropertyName("PaymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("PaymentStatus")]
        public string PaymentStatus { get; set; }

        [JsonPropertyName("DepositRefunded")]
        public bool DepositRefunded { get; set; }
    }

    public class PaymentInsertModel
    {
        [JsonPropertyName("TransactionID")]
        public int TransactionID { get; set; }

        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("PaymentMethod")]
        public string PaymentMethod { get; set; }
    }
}
