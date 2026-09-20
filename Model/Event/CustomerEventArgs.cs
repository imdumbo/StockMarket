namespace StockMarket.Model.Event
{
    public class CustomerEventArgs: EventArgs
    {
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime Timestamp { get; set; }

        public CustomerEventArgs(Guid customerID, string customerName)
        {
            CustomerID = customerID;
            CustomerName = customerName;
            Timestamp = DateTime.UtcNow;
        }
    }
}
