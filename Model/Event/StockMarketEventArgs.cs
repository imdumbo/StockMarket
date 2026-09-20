namespace StockMarket.Model.Event
{
    public class StockTransactionEventArgs : EventArgs
    {
        public Guid CustomerId { get; }
        public Guid StockId { get; }
        public string StockName { get; }
        public decimal Price { get; }
        public DateTime Timestamp { get; }

        public StockTransactionEventArgs(Guid customerId, Guid stockId, string stockName, decimal price)
        {
            CustomerId = customerId;
            StockId = stockId;
            StockName = stockName;
            Price = price;
            Timestamp = DateTime.UtcNow;
        }
    }
}
