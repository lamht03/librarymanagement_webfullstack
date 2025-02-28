using System;
using System.Text.Json.Serialization;

namespace LibraryManagement.Models
{
    public class Transaction
    {
        [JsonPropertyName("TransactionID")]
        public int TransactionID { get; set; }

        [JsonPropertyName("BookID")]
        public int BookID { get; set; }

        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("BorrowDate")]
        public DateTime BorrowDate { get; set; }

        [JsonPropertyName("DueDate")]
        public DateTime DueDate { get; set; }

        [JsonPropertyName("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        [JsonPropertyName("DepositAmount")]
        public decimal DepositAmount { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }
    }

    public class TransactionInsertModel
    {
        [JsonPropertyName("BookID")]
        public int BookID { get; set; }

        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("BorrowDate")]
        public DateTime BorrowDate { get; set; }

        [JsonPropertyName("DueDate")]
        public DateTime DueDate { get; set; }

        [JsonPropertyName("DepositAmount")]
        public decimal DepositAmount { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }
    }

    public class TransactionUpdateModel
    {
        [JsonPropertyName("TransactionID")]
        public int TransactionID { get; set; }

        [JsonPropertyName("BorrowDate")]
        public DateTime BorrowDate { get; set; }

        [JsonPropertyName("DueDate")]
        public DateTime DueDate { get; set; }


        [JsonPropertyName("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }
    }
}
