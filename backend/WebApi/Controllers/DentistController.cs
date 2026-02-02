using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;
using TelemedicinaOdonto.Infrastructure.Data;

namespace TelemedicinaOdonto.WebApi.Controllers;

[ApiController]
[Route("dentist")]
[Authorize(Roles = "Dentist")]
public class DentistController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly AppDbContext _dbContext;

    public DentistController(IAppointmentService appointmentService, AppDbContext dbContext)
    {
        _appointmentService = appointmentService;
        _dbContext = dbContext;
    }

    [HttpGet("appointments")]
    public async Task<ActionResult<IReadOnlyList<AppointmentResponse>>> GetAppointments()
    {
        var dentistId = GetDentistId();
        return Ok(await _appointmentService.GetDentistAppointmentsAsync(dentistId));
    }

    [HttpGet("appointments/{id:guid}")]
    public async Task<ActionResult<AppointmentResponse>> GetAppointment(Guid id)
    {
        var dentistId = GetDentistId();
        return Ok(await _appointmentService.GetDentistAppointmentAsync(dentistId, id));
    }

    private Guid GetDentistId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        var dentist = _dbContext.Dentists.FirstOrDefault(d => d.UserId == userId)
                      ?? throw new InvalidOperationException("Odontólogo no encontrado");
        return dentist.DentistId;
    }
}
