using StockMarket.Model.Event;

namespace StockMarket.Process
{
    public class StockMarketEventHandlers
    {
        private readonly string _logFilePath;

        public StockMarketEventHandlers(string logDirectory)
        {
            _logFilePath = Path.Combine(logDirectory, "stockmarket.log");
        }

        public void Subscribe(Transaction transaction)
        {
            transaction.StockPurchased += OnStockPurchased;
            transaction.StockSold += OnStockSold;
            transaction.PriceChanged += OnPriceChanged;
        }

        public void Unsubscribe(Transaction transaction)
        {
            transaction.StockPurchased -= OnStockPurchased;
            transaction.StockSold -= OnStockSold;
            transaction.PriceChanged -= OnPriceChanged;
        }

        private void OnStockPurchased(object? sender, StockTransactionEventArgs e)
        {
            string message = $"[PURCHASE] {e.Timestamp:HH:mm:ss} - Customer {e.CustomerId:N} bought {e.StockName} @ {e.Price:C}";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  >> {message}");
            Console.ResetColor();
            LogToFile(message);
        }

        private void OnStockSold(object? sender, StockTransactionEventArgs e)
        {
            string message = $"[SALE] {e.Timestamp:HH:mm:ss} - Customer {e.CustomerId:N} sold {e.StockName} @ {e.Price:C}";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  >> {message}");
            Console.ResetColor();
            LogToFile(message);
        }

        private void OnPriceChanged(object? sender, PriceChangedEventArgs e)
        {
            string direction = e.NewPrice > e.OldPrice ? "UP" : "DOWN";
            string message = $"[PRICE] {e.StockName} went {direction}: {e.OldPrice:C} -> {e.NewPrice:C} ({e.ChangePercent:+0.00;-0.00}%)";
            Console.ForegroundColor = e.NewPrice > e.OldPrice ? ConsoleColor.Cyan : ConsoleColor.Red;
            Console.WriteLine($"  >> {message}");
            Console.ResetColor();
            LogToFile(message);
        }

        private void LogToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"{DateTime.Now:yyyy-MM-dd} {message}{Environment.NewLine}");
            }
            catch
            {
            }
        }
    }
}
