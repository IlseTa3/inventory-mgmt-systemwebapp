using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class LocationStock
    {
        public int Id { get; set; }

        [Display(Name = "Name Location")]
        public string NameLocation { get; set; }

        [Display(Name = "Location Address")]
        public string LocationAddress { get; set; }

        [Display(Name = "Postalcode")]
        public string PostalCode { get; set; }

        public string Municipality { get; set; }
        public string Country { get; set; }

        // 1 adress per stock location, but there can be many stocks

        [ValidateNever]
        public virtual ICollection<ProductLocationStock> ProductLocationStocks { get; set; }
    }
}
