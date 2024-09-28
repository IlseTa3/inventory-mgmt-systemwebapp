namespace InventoryManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public string ProductNr { get; set; }
        public decimal Price { get; set; }

        // Foreign Key with Supplier
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        // 1 product can be stocked in different other Stock-locations
        public virtual ICollection<ProductLocationStock> ProductLocationStocks { get; set; }
    }
}
