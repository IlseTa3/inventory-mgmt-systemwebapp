using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class ProductLocationStock
    {
        public int Id { get; set; }

        //foreign key product
        public int ProductId { get; set; }
        [ValidateNever]
        public virtual Product Product { get; set; }


        //foreign key locationstock
        public int LocationStockId { get; set; }


        [ValidateNever]
        [Display(Name = "Stock location")]

        public virtual LocationStock LocationStock { get; set; }

        [Display(Name = "Total in Stock")]
        public int TotalInStock { get; set; }
    }
}
