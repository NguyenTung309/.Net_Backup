using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Models;
using Services;
using Microsoft.Extensions.Logging;

namespace ProductManagementMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Login page accessed");
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountMember model)
        {
            ModelState.Remove("FullName");
            ModelState.Remove("MemberId");


            _logger.LogInformation("Login attempt started for email: {Email}", model?.EmailAddress);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid");

                try
                {
                    var user = _accountService.GetAccountById(model.EmailAddress);
                    _logger.LogInformation("User lookup completed. User found: {UserFound}", user != null);

                    if (user != null)
                    {
                        _logger.LogDebug("User details - Email: {Email}, FullName: {FullName}",
                            user.EmailAddress, user.FullName);
                        _logger.LogDebug("Password match: {PasswordMatch}",
                            user.MemberPassword == model.MemberPassword);
                    }
                    else
                    {
                        _logger.LogWarning("No user found with email: {Email}", model.EmailAddress);
                    }

                    if (user != null && user.MemberPassword == model.MemberPassword)
                    {
                        // Store user information in session
                        HttpContext.Session.SetString("UserId", user.MemberId);
                        HttpContext.Session.SetString("Username", user.FullName);

                        _logger.LogInformation("Session created for user: {FullName} (ID: {MemberId})",
                            user.FullName, user.MemberId);
                        _logger.LogInformation("Login successful - Redirecting to Products/Index");

                        return RedirectToAction("Index", "Products");
                    }
                    else
                    {
                        _logger.LogWarning("Login failed - Invalid credentials for email: {Email}",
                            model.EmailAddress);
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during login for email: {Email}",
                        model.EmailAddress);
                    ModelState.AddModelError("", "An error occurred during login. Please try again.");
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Model error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            _logger.LogInformation("Returning to login view");
            return View(model);
        }

        public IActionResult Logout()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("Username");

            _logger.LogInformation("Logout requested for user: {Username} (ID: {UserId})", username, userId);

            HttpContext.Session.Clear(); // Clear session data

            _logger.LogInformation("Session cleared - Redirecting to Login page");
            return RedirectToAction("Login");
        }
    }
}