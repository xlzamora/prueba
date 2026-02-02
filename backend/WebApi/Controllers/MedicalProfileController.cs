using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.WebApi.Controllers;

[ApiController]
[Route("patients")]
public class MedicalProfileController : ControllerBase
{
    private readonly IMedicalProfileService _medicalProfileService;

    public MedicalProfileController(IMedicalProfileService medicalProfileService)
    {
        _medicalProfileService = medicalProfileService;
    }

    [Authorize(Roles = "Patient")]
    [HttpPost("{id:guid}/medical-profile")]
    public async Task<ActionResult<MedicalProfileResponse>> Upsert(Guid id, MedicalProfileRequest request)
    {
        return Ok(await _medicalProfileService.UpsertAsync(id, request));
    }

    [Authorize(Roles = "Dentist,Admin")]
    [HttpGet("{id:guid}/medical-profile")]
    public async Task<ActionResult<MedicalProfileResponse>> Get(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "Dentist";
        return Ok(await _medicalProfileService.GetAsync(id, userId, role));
    }
}
