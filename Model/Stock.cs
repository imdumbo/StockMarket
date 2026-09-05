namespace EventService.Model
{
    public class Stock
    {
        public Guid stockID { get; set; } = Guid.NewGuid();
        public string stockName { get; set; }
        public double currentPrice { get; set; }
        public double previousPrice { get; set; }
    }
}
