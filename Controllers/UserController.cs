using Crud_Application.Models;
using Crud_Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Crud_Application.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /User/Index
        public IActionResult Index()
        {
            List<User> users = _userService.GetAllUsers();
            return View(users);
        }

        // GET: /User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the same email already exists
                var existingUser = _userService.GetUserByEmail(user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "A user with the same email already exists.");
                    return View(user);
                }

                // No user with the same email exists, so create the new user
                _userService.CreateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: /User/Edit/1
        public IActionResult Edit(int id)
        {
            User user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/1
        [HttpPost]
        public IActionResult Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _userService.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: /User/Delete/1
        public IActionResult Delete(int id)
        {
            User user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/1
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _userService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /User/Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /User/Login
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the provided credentials are valid
                if (_userService.IsValidUser(model.Email, model.Password))
                {
                    User user = _userService.GetUserByEmail(model.Email);

                    // Create a claims identity for the authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    // Sign in the user
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties).Wait();

                    // Redirect to the desired page after successful login
                    return RedirectToAction("Index", "Home");
                }

                // Invalid credentials
                ModelState.AddModelError(string.Empty, "Invalid email or password");
            }

            // Model is not valid, show the login view with errors
            return View(model);
        }

        // GET: /User/Logout
        public async Task<IActionResult> Logout()
        {
            // Clear the user's session and sign out
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Perform any additional logout logic if needed

            return RedirectToAction("Login", "User");
        }
    }
}
