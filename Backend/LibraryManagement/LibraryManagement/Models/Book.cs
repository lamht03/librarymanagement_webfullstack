using System.Text.Json.Serialization;

namespace LibraryManagement.Models
{
    public class Book
    {
        [JsonPropertyName("BookID")]
        public int BookID { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Author")]
        public string Author { get; set; }

        [JsonPropertyName("Genre")]
        public string Genre { get; set; } = "Unknown";

        [JsonPropertyName("PublishedDate")]
        public DateTime PublishedDate { get; set; }

        [JsonPropertyName("TotalQuantity")]
        public int TotalQuantity { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("BorrowedQuantity")]
        public int BorrowedQuantity { get; set; }

        [JsonPropertyName("AvailableQuantity")]
        public int AvailableQuantity { get; set; }
    }

    public class BookInsertModel
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Author")]
        public string Author { get; set; }

        [JsonPropertyName("Genre")]
        public string Genre { get; set; } = "Unknown";

        [JsonPropertyName("PublishedDate")]
        public DateTime PublishedDate { get; set; }

        [JsonPropertyName("TotalQuantity")]
        public int TotalQuantity { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }
    }

    public class BookUpdateModel
    {
        [JsonPropertyName("BookID")]
        public int BookID { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Author")]
        public string Author { get; set; }

        [JsonPropertyName("Genre")]
        public string Genre { get; set; } = "Unknown";

        [JsonPropertyName("PublishedDate")]
        public DateTime PublishedDate { get; set; }

        [JsonPropertyName("TotalQuantity")]
        public int TotalQuantity { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

    }

}