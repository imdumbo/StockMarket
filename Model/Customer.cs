namespace EventService.Model
{
    public class Customer
    {
        public Guid CustomerID { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; }
    }
}
