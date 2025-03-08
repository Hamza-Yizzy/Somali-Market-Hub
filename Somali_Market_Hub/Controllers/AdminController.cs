// File: Controllers/AdminController.cs
using Microsoft.AspNetCore.Mvc;
using Somali_Market_Hub.Models;
using Somali_Market_Hub.Repositories;
using System.Threading.Tasks;

namespace Somali_Market_Hub.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestId(string role)
        {
            string prefix = role switch
            {
                "Admin" => "SA",
                "Provider" => "SP",
                "Customer" => "SC",
                _ => ""
            };

            var lastUser = await _userRepository.GetLastUserByRoleAsync(role);
            int nextNumber = lastUser != null ? int.Parse(lastUser.Id.Substring(2)) + 1 : 1;
            return Content($"{prefix}{nextNumber:D2}");
        }

        public async Task<IActionResult> ListUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return View(users);
        }

        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.AddUserAsync(user);
                return RedirectToAction("ListUsers");
            }
            return View(user);
        }

        [HttpGet("Admin/EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.UpdateUserAsync(user);
                return RedirectToAction("ListUsers");
            }
            return View(user);
        }
        [HttpDelete("Admin/DeleteUserJson/{id}")]
        public async Task<IActionResult> DeleteUserJson(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            await _userRepository.DeleteUserAsync(id);
            return Ok(new { message = "User deleted successfully." });
        }

    }
}