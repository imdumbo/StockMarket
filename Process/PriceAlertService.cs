using StockMarket.Model.Event;

namespace StockMarket.Process
{
    public class PriceAlertService
    {
        private readonly decimal _alertThresholdPercent;

        public PriceAlertService(decimal alertThresholdPercent = 3.0m)
        {
            _alertThresholdPercent = alertThresholdPercent;
        }

        public void Subscribe(Transaction transaction)
        {
            transaction.PriceChanged += OnPriceChanged;
        }

        public void Unsubscribe(Transaction transaction)
        {
            transaction.PriceChanged -= OnPriceChanged;
        }

        private void OnPriceChanged(object? sender, PriceChangedEventArgs e)
        {
            
            if (Math.Abs(e.ChangePercent) >= _alertThresholdPercent)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"  !! ALERT: {e.StockName} moved {e.ChangePercent:+0.00;-0.00}% - exceeds {_alertThresholdPercent}% threshold!");
                Console.ResetColor();
            }
        }
    }
}
