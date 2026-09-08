using EventService.DAL;
using EventService.Model;

namespace EventService.Process
{
    public class StockProcess
    {
        private readonly MyDataBaseProcess _dalProcess;
        private readonly Transaction _transaction;

        // Accept external MyDataBaseProcess so multiple processors share same storage directory
        public StockProcess(MyDataBaseProcess? dalProcess = null)
        {
            _dalProcess = dalProcess ?? new MyDataBaseProcess();
            _transaction = new Transaction(_dalProcess);
        }

        public string CreateStock(string? stockName, decimal currentPrice)
        {
            if (string.IsNullOrWhiteSpace(stockName) || currentPrice < 0)
            {
                return "Stock name and a non-negative price are required.";
            }

            stockName = stockName.Trim();
            if (DataBase.Stocks.Any(x => x.StockName.Equals(stockName, StringComparison.OrdinalIgnoreCase)))
            {
                return "Stock already exists";
            }

            Stock stock = new() { StockName = stockName, CurrentPrice = currentPrice };
            DataBase.Stocks.Add(stock);
            _dalProcess.SaveStockChanges();
            return $"Stock created with ID: {stock.StockId}";
        }

        internal void BuyStock(Guid customerId)
        {
            Guid stockId = SelectStockId();
            if (stockId != Guid.Empty)
            {
                Console.WriteLine(_transaction.BuyStock(customerId, stockId));
            }
        }

        internal void SellStock(Guid customerId)
        {
            Guid stockId = SelectStockId();
            if (stockId != Guid.Empty)
            {
                Console.WriteLine(_transaction.SellStock(customerId, stockId));
            }
        }

        private static Guid SelectStockId()
        {
            Console.WriteLine("Available stocks:");
            foreach (var stock in DataBase.Stocks)
            {
                Console.WriteLine($"{stock.StockName}     | Current: {stock.CurrentPrice:C}     | Previous: {stock.PreviousPrice:C}");
            }

            while (true)
            {
                Console.WriteLine("Please enter stock name (or EXIT):");
                string? stockName = Console.ReadLine()?.Trim();
                if (string.Equals(stockName, "EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    return Guid.Empty;
                }

                Stock? stock = DataBase.Stocks.FirstOrDefault(x =>
                    x.StockName.Equals(stockName, StringComparison.OrdinalIgnoreCase));
                if (stock is not null)
                {
                    return stock.StockId;
                }

                Console.WriteLine("Please enter a valid stock name.");
            }
        }
    }
}
