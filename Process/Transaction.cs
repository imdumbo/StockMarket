using StockMarket.DAL;
using StockMarket.Model;
using StockMarket.Model.Event;

namespace StockMarket.Process
{
    public class Transaction
    {
        private readonly MyDataBaseProcess _dalProcess;

        public event EventHandler<StockTransactionEventArgs>? StockPurchased;
        public event EventHandler<StockTransactionEventArgs>? StockSold;
        public event EventHandler<PriceChangedEventArgs>? PriceChanged;

        public Transaction(MyDataBaseProcess? dalProcess = null)
        {
            _dalProcess = dalProcess ?? new MyDataBaseProcess();
        }

        public string BuyStock(Guid customerId, Guid stockId)
        {
            if (!DataBase.Customers.Any(c => c.CustomerId == customerId))
            {
                return "Customer was not found.";
            }

            Stock? stock = DataBase.Stocks.FirstOrDefault(s => s.StockId == stockId);
            if (stock is null)
            {
                return "Stock was not found.";
            }

            Inventory inventory = DataBase.Inventories.FirstOrDefault(i => i.CustomerId == customerId)
                ?? CreateInventory(customerId);

            if (inventory.StockIds.Contains(stockId))
            {
                return "Customer already owns this stock.";
            }

            inventory.StockIds.Add(stockId);
            _dalProcess.SaveInventoryChanges();

            OnStockPurchased(new StockTransactionEventArgs(
                customerId, stockId, stock.StockName, stock.CurrentPrice));

            UpdateStockPrice(stock, 1.05m);

            return "Stock purchased successfully.";
        }

        public string SellStock(Guid customerId, Guid stockId)
        {
            Inventory? inventory = DataBase.Inventories.FirstOrDefault(i => i.CustomerId == customerId);
            if (inventory is null || !inventory.StockIds.Remove(stockId))
            {
                return "Customer does not own this stock.";
            }

            Stock? stock = DataBase.Stocks.FirstOrDefault(s => s.StockId == stockId);
            _dalProcess.SaveInventoryChanges();

            if (stock is not null)
            {
                OnStockSold(new StockTransactionEventArgs(
                    customerId, stockId, stock.StockName, stock.CurrentPrice));

                UpdateStockPrice(stock, 0.95m);
            }

            return "Stock sold successfully.";
        }

        private void UpdateStockPrice(Stock stock, decimal multiplier)
        {
            decimal oldPrice = stock.CurrentPrice;
            stock.PreviousPrice = oldPrice;
            stock.CurrentPrice = Math.Round(stock.CurrentPrice * multiplier, 2);
            _dalProcess.SaveStockChanges();

            OnPriceChanged(new PriceChangedEventArgs(stock.StockName, oldPrice, stock.CurrentPrice));
        }

        protected virtual void OnStockPurchased(StockTransactionEventArgs e)
        {
            StockPurchased?.Invoke(this, e);
        }

        protected virtual void OnStockSold(StockTransactionEventArgs e)
        {
            StockSold?.Invoke(this, e);
        }

        protected virtual void OnPriceChanged(PriceChangedEventArgs e)
        {
            PriceChanged?.Invoke(this, e);
        }

        private static Inventory CreateInventory(Guid customerId)
        {
            Inventory inventory = new() { CustomerId = customerId };
            DataBase.Inventories.Add(inventory);
            return inventory;
        }
    }
}