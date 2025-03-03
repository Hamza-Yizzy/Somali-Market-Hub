using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Somali_Market_Hub.Web.Version2.Data;
using Somali_Market_Hub.Web.Version2.Models.Domain;

namespace Somali_Market_Hub.Controllers
{
    public class AdminController : Controller
    {
        private readonly SMHDbContext context;

        public AdminController(SMHDbContext context)
        {
            this.context = context;
        }
        public IActionResult CreateUser()
        {
            ViewData["Roles"] = new SelectList(context.Tbl_Roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([Bind("Id, Fullname, Email, Username, Password, RoleId")] UserAccount account)
        {
            if (ModelState.IsValid)
            {
                context.Tbl_UserAccounts.Add(account);
                await context.SaveChangesAsync();
                return RedirectToAction("ListUsers");
            }
            return View(account);
        }
        public async Task<IActionResult> ListUsers()
        {
            var userlist = await context.Tbl_UserAccounts.
                                                          Include(r => r.Roles).
                                                          ToListAsync();
            return View(userlist);
        }
        public async Task<IActionResult> EditUser(int id)
        {
            ViewData["Roles"] = new SelectList(context.Tbl_Roles, "Id", "Name");
            var user = await context.Tbl_UserAccounts.FirstOrDefaultAsync(i => i.Id == id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser([Bind("Id, Fullname, Email, Username, Password, RoleId")] UserAccount account)
        {
            if (ModelState.IsValid)
            {
                context.Update(account);
                await context.SaveChangesAsync();
                return RedirectToAction("ListUsers");
            }
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await context.Tbl_UserAccounts.FindAsync(id);
            if (user != null)
            {
                context.Tbl_UserAccounts.Remove(user);
                await context.SaveChangesAsync();
                return RedirectToAction("ListUsers");
            }
            return View(user);
        }

    }
}
