using StockMarket.Model.Event;
using StockMarket.Process;

Console.WriteLine("**--Stock Market Started--**");

string dataDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwRoot"));

var dal = new MyDataBaseProcess(dataDirectory);
dal.Initialize();

var customerProcess = new CustomerProcess(dal);
var stockProcess = new StockProcess(dal);

var eventLogger = new StockMarketEventHandlers(dataDirectory);
var priceAlerts = new PriceAlertService(alertThresholdPercent: 2.0m);
var customerAlert = new CustomerAlertService(dataDirectory);

eventLogger.Subscribe(stockProcess.Transaction);
priceAlerts.Subscribe(stockProcess.Transaction);
customerAlert.Subscribe(customerProcess);

SeedSampleData(customerProcess, stockProcess);
RunConsoleApplication(customerProcess, stockProcess);

eventLogger.Unsubscribe(stockProcess.Transaction);
priceAlerts.Unsubscribe(stockProcess.Transaction);
customerAlert.Unsubscribe(customerProcess);

static void SeedSampleData(CustomerProcess customerProcess, StockProcess stockProcess)
{
    customerProcess.CreateCustomer("John Doe");
    customerProcess.CreateCustomer("Jane Smith");
    customerProcess.CreateCustomer("Alice Johnson");

    Console.WriteLine(stockProcess.CreateStock("AAPL", 150.00m));
    Console.WriteLine(stockProcess.CreateStock("GOOGL", 2500.00m));
    Console.WriteLine(stockProcess.CreateStock("MSFT", 300.00m));
}

static void RunConsoleApplication(CustomerProcess customerProcess, StockProcess stockProcess)
{
    while (true)
    {
        Console.WriteLine("\nPlease enter your name, or type EXIT to close:");
        string? customerName = Console.ReadLine()?.Trim();

        if (string.Equals(customerName, "EXIT", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        Guid customerId = customerProcess.FindCustomerId(customerName);
        if (customerId == Guid.Empty)
        {
            Console.WriteLine("Customer was not found. Create a new customer? (Y/N)");
            if (!string.Equals(Console.ReadLine()?.Trim(), "Y", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            customerId = customerProcess.CreateCustomer(customerName);
            if (customerId == Guid.Empty)
            {
                continue;
            }
        }

        while (true)
        {
            Console.WriteLine("Enter BUY, SELL, NEW LOGIN, or EXIT:");
            string? command = Console.ReadLine()?.Trim();

            if (string.Equals(command, "BUY", StringComparison.OrdinalIgnoreCase))
            {
                stockProcess.BuyStock(customerId);
            }
            else if (string.Equals(command, "SELL", StringComparison.OrdinalIgnoreCase))
            {
                stockProcess.SellStock(customerId);
            }
            else if (string.Equals(command, "NEW LOGIN", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            else if (string.Equals(command, "EXIT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid command. Use BUY, SELL, NEW LOGIN, or EXIT.");
            }
        }
    }
}