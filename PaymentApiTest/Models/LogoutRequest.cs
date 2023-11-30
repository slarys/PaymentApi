namespace PaymentApi.Models
{
    public class LogoutRequest
    {
        public int UserId { get; set; }
        public int TransactionId { get; set; }
    }
}
