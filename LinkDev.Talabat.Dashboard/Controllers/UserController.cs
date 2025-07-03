using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Talabat.DashBoard.Controllers
{
    public class UserController(RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var usersData = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.PhoneNumber
                })
                .ToListAsync();

            var users = new List<UserViewModel>();

            foreach (var user in usersData)
            {
                var roles = await _userManager.GetRolesAsync(
                    await _userManager.FindByIdAsync(user.Id)); 

                users.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    DisplayName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles
                });
            }

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(
                    r => new RoleViewModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                    }).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
