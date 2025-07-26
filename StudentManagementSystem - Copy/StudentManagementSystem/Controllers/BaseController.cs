using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        protected string GetCurrentUserName()
        {
            return User.Identity?.Name ?? string.Empty;
        }

        protected string GetCurrentUserFullName()
        {
            var fullNameClaim = User.FindFirst("FullName");
            return fullNameClaim?.Value ?? string.Empty;
        }

        protected string GetCurrentUserRole()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value ?? string.Empty;
        }

        protected bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        protected bool IsCurrentUser(int userId)
        {
            return GetCurrentUserId() == userId;
        }

        protected void SetSuccessMessage(string message)
        {
            TempData["Success"] = message;
        }

        protected void SetErrorMessage(string message)
        {
            TempData["Error"] = message;
        }

        protected void SetInfoMessage(string message)
        {
            TempData["Info"] = message;
        }

        protected void SetWarningMessage(string message)
        {
            TempData["Warning"] = message;
        }

        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        protected IActionResult RedirectToAccessDenied()
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        protected IActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}