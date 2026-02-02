using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.WebApi.Controllers;

[ApiController]
[Route("chat")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [Authorize]
    [HttpPost("sessions")]
    public async Task<ActionResult<ChatSessionResponse>> CreateSession(CreateChatSessionRequest request)
    {
        return Ok(await _chatService.CreateSessionAsync(request));
    }

    [Authorize]
    [HttpPost("sessions/{id:guid}/messages")]
    public async Task<ActionResult<ChatMessageResponse>> SendMessage(Guid id, SendChatMessageRequest request)
    {
        return Ok(await _chatService.AddMessageAsync(id, request));
    }

    [Authorize]
    [HttpGet("sessions/{id:guid}/messages")]
    public async Task<ActionResult<IReadOnlyList<ChatMessageResponse>>> GetMessages(Guid id)
    {
        return Ok(await _chatService.GetMessagesAsync(id));
    }
}
