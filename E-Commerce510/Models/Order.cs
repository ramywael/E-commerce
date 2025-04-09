namespace E_Commerce510.Models
{

    public enum enOrderStatus
    {
        Pending,
        InProgress,
        Shipped,
        Completed,
        Canceled,
    }
    public class Order
    {
        public int OrderId { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }

        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }

        public double OrderTotal { get; set; }
        public enOrderStatus Status { get; set; }
        public bool PaymentStatus { get; set; }

        public string? PaymentStripeId { get; set; }

        public string? SessionId { get; set; }
    }
}
