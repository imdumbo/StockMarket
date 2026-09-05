using EventService.Process;

Console.WriteLine("**--Stock Market Started--**");
// Initialize in-memory database process and save sample data to JSON files
var dal = new MyDataBaseProcess(@"F:\Net Project\EventService\wwwRoot");
await dal.InitializeAsync();

CustomerProcess customerProcess = new CustomerProcess(dal);
StockProcess stockProcess = new StockProcess(dal);
Transaction transaction = new Transaction();

// customer creation
customerProcess.createCustomer("John Doe").Wait();
customerProcess.createCustomer("Jane Smith").Wait();
customerProcess.createCustomer("Alice Johnson").Wait();

// stock creation
stockProcess.createStock("AAPL", 150.00).Wait();
stockProcess.createStock("GOOGL", 2500.00).Wait();
stockProcess.createStock("MSFT", 300.00).Wait();


