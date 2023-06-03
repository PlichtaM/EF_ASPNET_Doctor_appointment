using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebApplication2Context _context;

        public AccountController(WebApplication2Context context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: /Account/Login
        [HttpGet] // Dodaj atrybut [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId != null)
            {
                // User is already logged in, redirect to the profile page
                return RedirectToAction("Profile");
            }

            var loggedInUser = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == user.Username && u.Password == user.Password);

            if (loggedInUser != null)
            {
                // Set session context for the logged-in user
                HttpContext.Session.SetInt32("UserId", loggedInUser.Id);

                // Redirect the user to the profile page
                return RedirectToAction("Profile");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(user);
        }


        // GET: /Account/Profile
        public async Task<IActionResult> Profile()
        {
            // Check if the user is logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // User is not logged in, redirect to the login page
                return RedirectToAction("Login");
            }

            // User is logged in, retrieve profile data and pass it to the view
            var user = await _context.Users.Include(u => u.Appointments).FirstOrDefaultAsync(u => u.Id == userId);
            return View(user);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            // End the user session and clear the session context
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
