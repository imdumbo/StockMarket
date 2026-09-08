namespace EventService.Model
{
    public class Stock
    {
        public Guid StockId { get; set; } = Guid.NewGuid();
        public string StockName { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal PreviousPrice { get; set; }
    }
}
