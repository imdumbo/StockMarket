using EventService.DAL;
using EventService.Model;

namespace EventService.Process
{
    public class CustomerProcess
    {
        public MyDataBaseProcess dalProcess;
        public CustomerProcess(MyDataBaseProcess dalProcess = null)
        {
            this.dalProcess = dalProcess ?? new MyDataBaseProcess();
        }
        public async Task<string> createCustomer(string customerName)
        {
            Customer customer = new Model.Customer { CustomerName = customerName };
            DataBase.Customers.Add(customer);
            await dalProcess.SaveCustomerChangesAsync();
            return "Customer created with ID: " + customer.CustomerID.ToString();
        }

        public async Task<string> deleteCustomer(string customerName)
        {
            DataBase.Customers.RemoveAll(c => c.CustomerName == customerName);
            await dalProcess.SaveCustomerChangesAsync();
            return "Customer deleted successfully.";
        }
    }
}
