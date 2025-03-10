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

        // Helper function to generate the next ID
        public string GenerateUserId(int roleId)
        {
            string prefix = roleId switch
            {
                1 => "SA",  // Admin
                2 => "SP",  // Provider
                3 => "SC",  // Customer
                _ => "XX"   // Unknown Role
            };

            var lastUser = context.Tbl_UserAccounts
                .Where(u => u.RoleId == roleId)
                .OrderByDescending(u => u.Id)
                .FirstOrDefault();

            int nextNumber = lastUser != null ? int.Parse(lastUser.Id.Substring(2)) + 1 : 1;
            return $"{prefix}{nextNumber:D2}";
        }

        [HttpGet]
        public IActionResult GetNextUserId(int roleId)
        {
            string newId = GenerateUserId(roleId);
            return Json(new { userId = newId });
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
        
        public async Task<IActionResult> EditUser(string id)
        {
            ViewData["Roles"] = new SelectList(context.Tbl_Roles, "Id", "Name");
            var edit = await context.Tbl_UserAccounts.FirstOrDefaultAsync(i => i.Id == id);
            return View(edit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, [Bind("Id, FullName, Email, UserName, Password, RoleId, BusinessName, Location")] UserAccount account, IFormFile photoFile)
        {
            // ✅ Ensure the entity exists before updating
            var existingUser = await context.Tbl_UserAccounts.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound(); // User doesn't exist
            }

            // ✅ Update only modified fields
            existingUser.FullName = account.FullName;
            existingUser.Email = account.Email;
            existingUser.UserName = account.UserName;
            existingUser.RoleId = account.RoleId;
            existingUser.BusinessName = account.BusinessName;
            existingUser.Location = account.Location;

            if (!string.IsNullOrEmpty(account.Password))
            {
                existingUser.Password = HashPassword(account.Password); // Hash new password if changed
            }

            if (photoFile != null && photoFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photoFile.CopyToAsync(memoryStream);
                    existingUser.BusinessLogo = memoryStream.ToArray();
                }
            }

            await context.SaveChangesAsync();
            return RedirectToAction("UsersList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
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
