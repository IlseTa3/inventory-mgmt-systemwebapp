namespace InventoryManagementSystem.Models
{
    public class ProductLocationStock
    {
        public int Id { get; set; }

        //foreign key product
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }


        //foreign key locationstock
        public int LocationStockId { get; set; }
        public virtual LocationStock LocationStock { get; set; }

        public int TotalInStock { get; set; }
    }
}
