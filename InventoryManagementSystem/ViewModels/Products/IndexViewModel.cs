using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.ViewModels.Products
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name Product")]
        public string NameProduct { get; set; }
        public string ProductNr { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public string NameSupplier { get; set; }
    }
}
