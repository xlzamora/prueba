using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.WebApi.Controllers;

[ApiController]
[Route("triage")]
public class TriageController : ControllerBase
{
    private readonly ITriageService _triageService;

    public TriageController(ITriageService triageService)
    {
        _triageService = triageService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TriageResponse>> Evaluate(TriageRequest request)
    {
        return Ok(await _triageService.EvaluateAsync(request));
    }
}
