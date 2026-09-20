namespace StockMarket.Model.Event
{
    public class PriceChangedEventArgs : EventArgs
    {
        public string StockName { get; }
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }
        public decimal ChangePercent => OldPrice != 0 ? ((NewPrice - OldPrice) / OldPrice) * 100 : 0;

        public PriceChangedEventArgs(string stockName, decimal oldPrice, decimal newPrice)
        {
            StockName = stockName;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }
}
