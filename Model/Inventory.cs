namespace EventService.Model
{
    public class Inventory
    {
        public Guid customerID { get; set; }
        public List<Guid> stocks { get; set; }
    }
}
