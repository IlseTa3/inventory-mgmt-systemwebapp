namespace InventoryManagementSystem.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int TotalInStock { get; set; }

        // Foreign Key to Product
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        // Foreign Key to LocationStock
        public int LocationStockId { get; set; }
        public virtual LocationStock LocationStock { get; set; }
    }
}
