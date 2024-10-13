using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InventoryManagementSystem.ViewModels.Registrations
{

    public class IndexViewModel
    {
        [ValidateNever]
        public string Id { get; set; }
        public string PersonnelNr { get; set; }
        
        public string Firstname { get; set; }
       
        public string Lastname { get; set; }

        public string Email { get; set; }

        public string CellphoneNr { get; set; }

        public string Role { get; set; }
    }
}
