namespace LibraryManagement.Models
{
    public class Session
    {
        public int SessionID { get; set; }
        public int UserID { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
