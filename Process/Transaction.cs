using EventService.DAL;
using EventService.Model;

namespace EventService.Process
{
    public class Transaction
    {
        private readonly MyDataBaseProcess _dalProcess;

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

            if (!DataBase.Stocks.Any(s => s.StockId == stockId))
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
            return "Stock purchased successfully.";
        }

        public string SellStock(Guid customerId, Guid stockId)
        {
            Inventory? inventory = DataBase.Inventories.FirstOrDefault(i => i.CustomerId == customerId);
            if (inventory is null || !inventory.StockIds.Remove(stockId))
            {
                return "Customer does not own this stock.";
            }

            _dalProcess.SaveInventoryChanges();
            return "Stock sold successfully.";
        }

        private static Inventory CreateInventory(Guid customerId)
        {
            Inventory inventory = new() { CustomerId = customerId };
            DataBase.Inventories.Add(inventory);
            return inventory;
        }
    }
}