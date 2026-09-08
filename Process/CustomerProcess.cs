using EventService.DAL;
using EventService.Model;

namespace EventService.Process
{
    public class CustomerProcess
    {
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
            _dalProcess.SaveCustomerChanges();
            _dalProcess.SaveInventoryChanges();
            return true;
        }
    }
}
