using PaymentApiTest.Models;

namespace PaymentApi.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
