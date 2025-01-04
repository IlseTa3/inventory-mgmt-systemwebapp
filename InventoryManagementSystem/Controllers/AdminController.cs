using InventoryManagementSystem.Models;
using InventoryManagementSystem.ViewModels.Registrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{

    /*
     Admincontroller = to maintain all incoming registers for all employees.
    Depending on the PersonellNr, a new registered member will be provided a Role.
    This role gives a certain access to the platform.
    For instance: a "Stockemployee" can READ and UPDATE products, but cannot CREATE or DELETE products.
     
    An administrator can also CREATE new members, if asked my HR.
    The new member gets an email with a temporary password and can login.
    Best is that the new member changes his password.
     
     */
    [Authorize(Policy = "RequireAdministratorRole")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = from u in _userManager.Users
                        join ur in _context.UserRoles on u.Id equals ur.UserId
                        join r in _context.Roles on ur.RoleId equals r.Id
                        select new IndexViewModel
                        {
                            Id = u.Id,
                            PersonnelNr = u.PersonnelNr,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Email = u.Email,
                            CellphoneNr = u.CellphoneNr,
                            Role = r.Name
                        };

            return View(users);
        }

        //Create GET
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name");
            return View();
        }



        //Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonnelNr,Firstname,Lastname,Email,CellphoneNr,Role")] IndexViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = vm.Email, // Voeg deze regel toe
                    PersonnelNr = vm.PersonnelNr,
                    Firstname = vm.Firstname,
                    Lastname = vm.Lastname,
                    Email = vm.Email,
                    CellphoneNr = vm.CellphoneNr,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "RetroConst123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, vm.Role);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                //return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name");
            return View(vm);
        }


        // GET Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var registratie = await _context.Users.FindAsync(id);
            if (registratie == null)
            {
                return NotFound();
            }

            // Haal alle beschikbare rollen op voor de dropdown
            ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name");

            // Probeer de gebruikersrol op te halen, maar controleer of er een rol is
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == id);
            string roleName = userRole != null ?
                              (await _context.Roles.FindAsync(userRole.RoleId))?.Name :
                              "Role to be specified";  // Gebruik "Geen rol" als de gebruiker geen rol heeft

            return View(new IndexViewModel
            {
                PersonnelNr = registratie.PersonnelNr,
                Firstname = registratie.Firstname,
                Lastname = registratie.Lastname,
                Email = registratie.Email,
                CellphoneNr = registratie.CellphoneNr,
                Role = roleName
            });
        }


        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PersonnelNr,Firstname,Lastname,Email,CellphoneNr,Role")] IndexViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(vm.Id);

                // Update de gebruikersinformatie
                user.PersonnelNr = vm.PersonnelNr;
                user.Firstname = vm.Firstname;
                user.Lastname = vm.Lastname;
                user.Email = vm.Email;
                user.CellphoneNr = vm.CellphoneNr;

                // Haal de bestaande rol van de gebruiker op
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Verwijder de bestaande rollen (als er een rol is)
                if (currentRoles.Count > 0)
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                // Voeg de nieuwe rol toe
                var rolUpdate = vm.Role;
                var roleResult = await _userManager.AddToRoleAsync(user, rolUpdate);

                if (roleResult.Succeeded)
                {
                    // Sla de wijzigingen in de database op
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Als het model niet geldig is, keer terug naar de view met de foutmeldingen
            ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name");
            return View(vm);
        }


        //Loading Registrations for Ajax-call
        public IActionResult LoadAllRegistrations()
        {
            try
            {
                var registrationData = (from u in _userManager.Users
                                        join ur in _context.UserRoles on u.Id equals ur.UserId into userRoles
                                        from ur in userRoles.DefaultIfEmpty()
                                        join r in _context.Roles on ur.RoleId equals r.Id into roles
                                        from r in roles.DefaultIfEmpty()
                                        select new IndexViewModel
                                        {
                                            Id = u.Id,
                                            PersonnelNr = u.PersonnelNr,
                                            Firstname = u.Firstname,
                                            Lastname = u.Lastname,
                                            Email = u.Email,
                                            CellphoneNr = u.CellphoneNr,
                                            Role = r != null ? r.Name : "Role to be specified"
                                        }).ToList<IndexViewModel>();

                return Json(new { data = registrationData });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
