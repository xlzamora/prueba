using Microsoft.AspNetCore.Identity;

namespace TelemedicinaOdonto.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
