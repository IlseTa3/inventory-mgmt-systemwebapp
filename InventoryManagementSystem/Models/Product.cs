using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Name Product")]
        public string NameProduct { get; set; }
        public string ProductNr { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        // Foreign Key with Supplier
        public int SupplierId { get; set; }

        [ValidateNever]
        public virtual Supplier Supplier { get; set; }

        // 1 product can be stocked in different other Stock-locations

        [ValidateNever]
        public virtual ICollection<ProductLocationStock> ProductLocationStocks { get; set; }
    }
}
