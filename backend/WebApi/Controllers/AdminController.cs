using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.WebApi.Controllers;

[ApiController]
[Route("admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IKnowledgeBaseService _knowledgeBaseService;
    private readonly IAuditService _auditService;

    public AdminController(IKnowledgeBaseService knowledgeBaseService, IAuditService auditService)
    {
        _knowledgeBaseService = knowledgeBaseService;
        _auditService = auditService;
    }

    [HttpGet("kb")]
    public async Task<ActionResult<IReadOnlyList<KnowledgeBaseItemResponse>>> GetKb()
    {
        return Ok(await _knowledgeBaseService.GetAllAsync());
    }

    [HttpPost("kb")]
    public async Task<ActionResult<KnowledgeBaseItemResponse>> CreateKb(KnowledgeBaseItemRequest request)
    {
        return Ok(await _knowledgeBaseService.CreateAsync(request));
    }

    [HttpPut("kb/{id:guid}")]
    public async Task<ActionResult<KnowledgeBaseItemResponse>> UpdateKb(Guid id, KnowledgeBaseItemRequest request)
    {
        return Ok(await _knowledgeBaseService.UpdateAsync(id, request));
    }

    [HttpDelete("kb/{id:guid}")]
    public async Task<IActionResult> DeleteKb(Guid id)
    {
        await _knowledgeBaseService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("audit")]
    public async Task<ActionResult<IReadOnlyList<AuditLogResponse>>> GetAudit()
    {
        return Ok(await _auditService.GetAllAsync());
    }
}
