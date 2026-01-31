using Microsoft.AspNetCore.Identity;

namespace Task4WebApp.Models;

public class User : IdentityUser
{
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow.AddHours(3);
    public DateTime LastLoginDate { get; set; } = DateTime.UtcNow.AddHours(3);
    public string Status => LockoutEnd > DateTimeOffset.Now ? "Blocked" : !EmailConfirmed ? "Unverified" : "Active";
}