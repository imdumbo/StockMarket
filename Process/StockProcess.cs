using EventService.DAL;
using EventService.Model;

namespace EventService.Process
{
    public class StockProcess
    {
        public MyDataBaseProcess dalProcess;
        // Accept external MyDataBaseProcess so multiple processors share same storage directory
        public StockProcess(MyDataBaseProcess dalProcess = null)
        {
            this.dalProcess = dalProcess ?? new MyDataBaseProcess();
        }
        public async Task<string> createStock(string stockName, double currentPrice)
        {
            Stock stock = new Model.Stock { stockName = stockName, currentPrice = currentPrice, previousPrice = 0 };
            DataBase.Stocks.Add(stock);
            await dalProcess.SaveStockChangesAsync();
            return "Stock created with ID: " + stock.stockID.ToString();
        }
    }
}
