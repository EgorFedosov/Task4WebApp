using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4WebApp.Models;

namespace Task4WebApp.Controllers;

[Authorize]
public class HomeController(UserManager<User> userManager, SignInManager<User> signInManager) : Controller
{
    // NOTE: Sort users by last login time.
    public async Task<IActionResult> Index()
    {
        var currentUserId = userManager.GetUserId(User);
        if (currentUserId != null)
        {
            await userManager.Users
                .Where(u => u.Id == currentUserId)
                .ExecuteUpdateAsync(s 
                    => s.SetProperty(u => u.LastLoginDate, DateTime.UtcNow.AddHours(3)));
        }

        var users = await userManager.Users
            .OrderByDescending(u => u.LastLoginDate)
            .ToListAsync();

        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> Block(string[]? selectedIds)
    {
        if (selectedIds == null || selectedIds.Length == 0) return RedirectToAction(nameof(Index));

        // IMPORTANT: Block users by setting LockoutEnd.
        await userManager.Users
            .Where(u => ((IEnumerable<string>)selectedIds).Contains(u.Id))
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.LockoutEnd, DateTimeOffset.UtcNow.AddYears(100))
                .SetProperty(u => u.SecurityStamp, Guid.NewGuid().ToString()));

        var currentUserId = userManager.GetUserId(User);

        if (selectedIds.Contains(currentUserId))
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Unblock(string[]? selectedIds)
    {
        if (selectedIds == null || selectedIds.Length == 0) return RedirectToAction(nameof(Index));

        // NOTA BENE: Unblock users by removing lockout.
        await userManager.Users
            .Where(u => ((IEnumerable<string>)selectedIds).Contains(u.Id))
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.LockoutEnd, (DateTimeOffset?)null));

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string[]? selectedIds)
    {
        if (selectedIds == null || selectedIds.Length == 0) return RedirectToAction(nameof(Index));

        var currentUserId = userManager.GetUserId(User);
        
        // NOTE: Delete users from database.
        await userManager.Users
            .Where(u => ((IEnumerable<string>)selectedIds).Contains(u.Id))
            .ExecuteDeleteAsync();

        if (selectedIds.Contains(currentUserId))
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        return RedirectToAction(nameof(Index));
    }

    // IMPORTANT: Remove unverified accounts.
    [HttpPost]
    public async Task<IActionResult> DeleteUnverified()
    {
        var currentUser = await userManager.GetUserAsync(User);
        var isCurrentUserUnverified = currentUser is { EmailConfirmed: false };

        await userManager.Users
            .Where(u => !u.EmailConfirmed)
            .ExecuteDeleteAsync();

        if (!isCurrentUserUnverified) return RedirectToAction(nameof(Index));
        await signInManager.SignOutAsync();
        return RedirectToPage("/Account/Login", new { area = "Identity" });

    }
}