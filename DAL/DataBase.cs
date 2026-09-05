using EventService.Model;

namespace EventService.DAL
{
    public static class DataBase
    {
        public static List<Customer> Customers { get; set; } = new();
        public static List<Inventory> Inventories { get; set; } = new();
        public static List<Stock> Stocks { get; set; } = new();
    }
}
