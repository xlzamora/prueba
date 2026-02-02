namespace TelemedicinaOdonto.Application.DTOs;

public record RegisterPatientRequest(
    string Email,
    string Password,
    string FullName,
    string DocumentId,
    DateOnly BirthDate
);

public record LoginRequest(string Email, string Password);

public record AuthResponse(string Token, string UserId, string Role);

public record CurrentUserResponse(string UserId, string Email, string Role, Guid? PatientId, Guid? DentistId);
