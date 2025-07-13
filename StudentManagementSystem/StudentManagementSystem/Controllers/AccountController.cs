using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.ViewModels;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    //ValidateAntiForgeryToken ->  CSRF (Cross-Site Request Forgery)
    //We use it in the post while we are collecting data from the user
    //AllowAnonymous -> Entry is allowed without authentication on the action.
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        #region Authentication

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _userService.GetUserByNameAsync(model.Username);

                if (user == null || !user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة");
                    return View(model);
                }

                // Note: In production, use proper password hashing (BCrypt, Argon2, etc.)
                if (user.Password != model.Password)
                {
                    ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة");
                    return View(model);
                }

                await SignInUserAsync(user, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تسجيل الدخول");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel();
            await PopulateRolesDropDown();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateRolesDropDown();
                return View(model);
            }

            try
            {
                // Check if username already exists
                var existingUser = await _userService.GetUserByNameAsync(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "اسم المستخدم موجود بالفعل");
                    await PopulateRolesDropDown();
                    return View(model);
                }

                var user = new User
                {
                    Name = model.Name,
                    Username = model.Username,
                    Password = model.Password, // Note: Hash password in production
                    RoleId = model.RoleId,
                    IsActive = true,
                    CreatedBy = GetCurrentUserId()
                };

                await _userService.CreateUserAsync(user);
                TempData["Success"] = "تم إنشاء المستخدم بنجاح";
                return RedirectToAction("ManageUsers");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء المستخدم");
                await PopulateRolesDropDown();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        #endregion

        #region User Management

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var viewModel = users.Select(u => new UserManagementViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    RoleName = u.Role?.Name ?? "غير محدد",
                    Date = u.Date,
                    IsActive = u.IsActive,
                    CreatedBy = u.CreatedByUser?.Name ?? "غير محدد"
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تحميل بيانات المستخدمين";
                return View(new List<UserManagementViewModel>());
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["Error"] = "المستخدم غير موجود";
                    return RedirectToAction("ManageUsers");
                }

                var viewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive
                };

                await PopulateRolesDropDown();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تحميل بيانات المستخدم";
                return RedirectToAction("ManageUsers");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateRolesDropDown();
                return View(model);
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(model.Id);
                if (user == null)
                {
                    TempData["Error"] = "المستخدم غير موجود";
                    return RedirectToAction("ManageUsers");
                }

                // Check if username is taken by another user
                var existingUser = await _userService.GetUserByNameAsync(model.Username);
                if (existingUser != null && existingUser.Id != model.Id)
                {
                    ModelState.AddModelError("Username", "اسم المستخدم موجود بالفعل");
                    await PopulateRolesDropDown();
                    return View(model);
                }

                user.Name = model.Name;
                user.Username = model.Username;
                user.RoleId = model.RoleId;
                user.IsActive = model.IsActive;

                // Update password if provided
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    user.Password = model.NewPassword; // Note: Hash password in production
                }

                await _userService.UpdateUserAsync(user);
                TempData["Success"] = "تم تحديث المستخدم بنجاح";
                return RedirectToAction("ManageUsers");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تحديث المستخدم");
                await PopulateRolesDropDown();
                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == id)
                {
                    TempData["Error"] = "لا يمكن حذف المستخدم الحالي";
                    return RedirectToAction("ManageUsers");
                }

                var result = await _userService.DeleteUserAsync(id);
                if (result)
                {
                    TempData["Success"] = "تم حذف المستخدم بنجاح";
                }
                else
                {
                    TempData["Error"] = "فشل في حذف المستخدم";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء حذف المستخدم";
            }

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == id)
                {
                    TempData["Error"] = "لا يمكن تعطيل المستخدم الحالي";
                    return RedirectToAction("ManageUsers");
                }

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["Error"] = "المستخدم غير موجود";
                    return RedirectToAction("ManageUsers");
                }

                user.IsActive = !user.IsActive;
                await _userService.UpdateUserAsync(user);

                string status = user.IsActive ? "تم تفعيل" : "تم تعطيل";
                TempData["Success"] = $"{status} المستخدم بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تحديث حالة المستخدم";
            }

            return RedirectToAction("ManageUsers");
        }

        #endregion

        #region Profile Management

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var viewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تحميل الملف الشخصي";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = GetCurrentUserId();
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // Check if username is taken by another user
                var existingUser = await _userService.GetUserByNameAsync(model.Username);
                if (existingUser != null && existingUser.Id != userId)
                {
                    ModelState.AddModelError("Username", "اسم المستخدم موجود بالفعل");
                    return View(model);
                }

                user.Name = model.Name;
                user.Username = model.Username;

                // Update password if provided
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    user.Password = model.NewPassword; // Note: Hash password in production
                }

                await _userService.UpdateUserAsync(user);
                TempData["Success"] = "تم تحديث الملف الشخصي بنجاح";

                // Update claims if username changed
                if (User.Identity.Name != model.Username)
                {
                    await SignInUserAsync(user, false);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تحديث الملف الشخصي");
                return View(model);
            }
        }

        #endregion

        #region Access Denied

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion

        #region Helper Methods

        private async Task SignInUserAsync(User user, bool isPersistent)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"),
                new Claim("FullName", user.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = isPersistent ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        private async Task PopulateRolesDropDown()
        {
            var roles = await _roleService.GetActiveRolesAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
        }

        #endregion
    }
}