using StockMarket.Model.Event;

namespace StockMarket.Process
{
    public class CustomerAlertService
    {
        private readonly string _logFilePath;

        public CustomerAlertService(string logDirectory)
        {
            _logFilePath = Path.Combine(logDirectory, "stockmarket.log");
        }
        public void Subscribe(CustomerProcess customerProcess)
        {
            customerProcess.CustomerCreated += OnCustomerCreated;
            customerProcess.CustomerDeleted += OnCustomerDeleted;
        }

        public void Unsubscribe(CustomerProcess customerProcess)
        {
            customerProcess.CustomerCreated -= OnCustomerCreated;
            customerProcess.CustomerDeleted -= OnCustomerDeleted;
        }

        private void OnCustomerCreated(object sender, CustomerEventArgs e)
        {
            string message = $"[Added] {e.Timestamp:HH:mm:ss} - Customer {e.CustomerName} created with {e.CustomerID}";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  >> {message}");
            Console.ResetColor();
            LogToFile(message);
        }

        private void OnCustomerDeleted(object sender, CustomerEventArgs e)
        {
            string message = $"[Deleted] {e.Timestamp:HH:mm:ss} - Customer {e.CustomerName} deleted with {e.CustomerID}";
            Console.ForegroundColor = ConsoleColor.Red;
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
