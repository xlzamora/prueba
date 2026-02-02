using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Infrastructure.Data;
using TelemedicinaOdonto.Infrastructure.Identity;

namespace TelemedicinaOdonto.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;

    public AuthService(UserManager<ApplicationUser> userManager, AppDbContext dbContext, IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthResponse> RegisterPatientAsync(RegisterPatientRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, "Patient");

        var patient = new Patient
        {
            PatientId = Guid.NewGuid(),
            UserId = user.Id,
            FullName = request.FullName,
            DocumentId = request.DocumentId,
            BirthDate = request.BirthDate,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Patients.Add(patient);
        await _dbContext.SaveChangesAsync();

        var token = GenerateJwt(user, "Patient");
        return new AuthResponse(token, user.Id, "Patient");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new InvalidOperationException("Usuario no encontrado");
        var valid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!valid)
        {
            throw new InvalidOperationException("Credenciales inválidas");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Patient";
        var token = GenerateJwt(user, role);
        return new AuthResponse(token, user.Id, role);
    }

    public async Task<CurrentUserResponse> GetCurrentUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException("Usuario no encontrado");
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Patient";
        var patientId = _dbContext.Patients.FirstOrDefault(p => p.UserId == userId)?.PatientId;
        var dentistId = _dbContext.Dentists.FirstOrDefault(d => d.UserId == userId)?.DentistId;
        return new CurrentUserResponse(user.Id, user.Email ?? string.Empty, role, patientId, dentistId);
    }

    private string GenerateJwt(ApplicationUser user, string role)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
