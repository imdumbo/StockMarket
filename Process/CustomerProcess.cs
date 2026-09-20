using StockMarket.DAL;
using StockMarket.Model;
using StockMarket.Model.Event;

namespace StockMarket.Process
{
    public class CustomerProcess
    {
        public EventHandler<CustomerEventArgs>? CustomerCreated;
        public EventHandler<CustomerEventArgs>? CustomerDeleted;
        private readonly MyDataBaseProcess _dalProcess;

        public CustomerProcess(MyDataBaseProcess? dalProcess = null)
        {
            _dalProcess = dalProcess ?? new MyDataBaseProcess();
        }
        public Guid CreateCustomer(string? customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                Console.WriteLine("Customer name cannot be empty.");
                return Guid.Empty;
            }

            customerName = customerName.Trim();
            if (DataBase.Customers.Any(x => x.CustomerName.Equals(customerName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Customer already exists.");
                return Guid.Empty;
            }

            Customer customer = new() { CustomerName = customerName };
            DataBase.Customers.Add(customer);
            _dalProcess.SaveCustomerChanges();
            OnCustomerCreated(new CustomerEventArgs(customer.CustomerId, customer.CustomerName));
            Console.WriteLine($"Customer created with ID: {customer.CustomerId}");
            return customer.CustomerId;
        }

        public Guid FindCustomerId(string? customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                return Guid.Empty;
            }

            return DataBase.Customers
                .FirstOrDefault(c => c.CustomerName.Equals(customerName.Trim(), StringComparison.OrdinalIgnoreCase))?
                .CustomerId ?? Guid.Empty;
        }

        public bool DeleteCustomer(string? customerName)
        {
            Guid customerId = FindCustomerId(customerName);
            if (customerId == Guid.Empty)
            {
                return false;
            }

            DataBase.Customers.RemoveAll(c => c.CustomerId == customerId);
            DataBase.Inventories.RemoveAll(i => i.CustomerId == customerId);
            OnCustomerDeleted(new CustomerEventArgs(customerId, customerName));
            _dalProcess.SaveCustomerChanges();
            _dalProcess.SaveInventoryChanges();
            return true;
        }

        private void OnCustomerCreated(CustomerEventArgs e)
        {
            CustomerCreated?.Invoke(this, e);
        }
        public void OnCustomerDeleted(CustomerEventArgs e)
        {
            CustomerDeleted?.Invoke(this, e);
        }
    }
}
