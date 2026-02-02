using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.WebApi.Controllers;

[ApiController]
[Route("appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [Authorize]
    [HttpGet("available")]
    public async Task<ActionResult<IReadOnlyList<AvailableSlotResponse>>> GetAvailable([FromQuery] Guid dentistId, [FromQuery] DateTime date)
    {
        var request = new AvailableSlotsRequest(dentistId, date);
        return Ok(await _appointmentService.GetAvailableSlotsAsync(request));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AppointmentResponse>> Create(CreateAppointmentRequest request)
    {
        return Ok(await _appointmentService.CreateAppointmentAsync(request));
    }

    [Authorize]
    [HttpPut("{id:guid}/reschedule")]
    public async Task<ActionResult<AppointmentResponse>> Reschedule(Guid id, RescheduleAppointmentRequest request)
    {
        return Ok(await _appointmentService.RescheduleAsync(id, request));
    }

    [Authorize]
    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _appointmentService.CancelAsync(id);
        return NoContent();
    }
}
