using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Somali_Market_Hub.Data;
using Somali_Market_Hub.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Somali_Market_Hub.Controllers
{
    public class Admin : Controller
    {
        public readonly SMHDbContext context;

        public Admin(SMHDbContext context)
        {
            this.context = context;
        }
        // --------------------- API's and Facilitators ----------------------
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Convert bytes to hexadecimal
                }
                return builder.ToString();
            }
        }
        // [Get Image]
        public async Task<IActionResult> GetImage(int id)
        {
            var useraccount = await context.Tbl_UserAccounts.FindAsync(id);
            if (useraccount == null || useraccount.BusinessLogo == null)
            {
                return NotFound();
            }
            return File(useraccount.BusinessLogo, "image/jpeg"); // Serve image
        }

        // ---------------- CRUD Operations Code ----------------------

        public async Task<IActionResult> UsersList()
        {
            var users = await context.Tbl_UserAccounts.ToListAsync();
            return View(users);
        }

        public IActionResult CreateUser()
        {
            ViewData["Roles"] = new SelectList(context.Tbl_Roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("Id, FullName, Email, UserName, Password, RoleId, BusinessName, Location")] UserAccount account, IFormFile photoFile)
        {
            if (ModelState.IsValid)
            {
                if (photoFile != null && photoFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photoFile.CopyToAsync(memoryStream);
                        account.BusinessLogo = memoryStream.ToArray(); // Convert image to byte array
                    }
                }

                account.Password = HashPassword(account.Password);

                context.Tbl_UserAccounts.Add(account);
                await context.SaveChangesAsync();
                return RedirectToAction("UsersList");
            }
            return View(account);
        }
        
        public async Task<IActionResult> EditUser(int id)
        {
            ViewData["Roles"] = new SelectList(context.Tbl_Roles, "Id", "Name");
            var edit = await context.Tbl_UserAccounts.FirstOrDefaultAsync(i => i.Id == id);
            return View(edit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser([Bind("Id, FullName, Email, UserName, Password, RoleId, BusinessName, Location")] UserAccount account, IFormFile photoFile)
        {
            if (ModelState.IsValid)
            {
                if (photoFile != null && photoFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photoFile.CopyToAsync(memoryStream);
                        account.BusinessLogo = memoryStream.ToArray(); // Convert image to byte array
                    }
                }

                account.Password = HashPassword(account.Password);

                context.Tbl_UserAccounts.Update(account);
                await context.SaveChangesAsync();
                return RedirectToAction("UsersList");
            }
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var users = await context.Tbl_UserAccounts.FirstOrDefaultAsync(i => i.Id == id);
            if (users != null)
            {
                context.Tbl_UserAccounts.Remove(users);
                await context.SaveChangesAsync();

                // Show Success Message

                return RedirectToAction("UsersList");
            }
            // Show Error Message
            return View(null);

        }
    }
}
