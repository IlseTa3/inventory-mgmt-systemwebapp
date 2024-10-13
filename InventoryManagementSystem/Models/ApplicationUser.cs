using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string PersonnelNr { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string CellphoneNr { get; set; }
    }
}
