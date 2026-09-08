namespace EventService.Model
{
    public class Inventory
    {
        public Guid CustomerId { get; set; }
        public List<Guid> StockIds { get; set; } = new();
    }
}
