namespace InventoryManagementSystem.Models
{
    public class LocationStock
    {
        public int Id { get; set; }
        public string NameLocation { get; set; }
        public string LocationAddress { get; set; }
        public string PostalCode { get; set; }
        public string Commune { get; set; }
        public string Country { get; set; }

        // 1 adress per stock location, but there can be many stocks
        public virtual ICollection<ProductLocationStock> ProductLocationStocks { get; set; }
    }
}
