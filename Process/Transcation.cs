using EventService.DAL;
using System.Security.Principal;

namespace EventService.Process
{
    public class Transaction
    {
        public void buyStock(Guid custID, Guid stockID)
        {
            var customer = DataBase.Customers.FirstOrDefault(c => c.CustomerID == custID);
            var stock = DataBase.Stocks.FirstOrDefault(s => s.stockID == stockID);
            DataBase.Inventories.Where(i=>i.customerID == custID).FirstOrDefault()?.stocks.Add(stockID);
        }

        public void sellStock(Guid custID, Guid stockID)
        {
            var customer = DataBase.Customers.FirstOrDefault(c => c.CustomerID == custID);
            var stock = DataBase.Stocks.FirstOrDefault(s => s.stockID == stockID);
            DataBase.Inventories.Where(i => i.customerID == custID).FirstOrDefault()?.stocks.Remove(stockID);
        }
    }
}
