using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Chat_Website.Models.DataBase;
using System.Security.Claims;

namespace Chat_Website.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(UserManager<UserApplication> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(IFormFile? profileImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (profileImage != null && profileImage.Length > 0)
            {
                // Ensure directory exists
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ProfileImages");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, user.ProfileImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Save new image
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(fileStream);
                }

                user.ProfileImageUrl = "/ProfileImages/" + uniqueFileName;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Error updating profile picture.");
                    return View(user);
                }
            }

            return RedirectToAction(nameof(Profile));
        }
    }
}