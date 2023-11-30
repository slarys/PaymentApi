using PaymentApi.Models;

namespace PaymentApiTest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public decimal Balance { get; set; }
        public List<Token> Tokens { get; set; } = new List<Token>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

