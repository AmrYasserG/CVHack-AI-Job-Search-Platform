using CVHack.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API
{
    [Route("api/tickets")]
    [ApiController]
    [Authorize(Policy = "JobSeekerOnly")]
    public class SupportTicketsController : ControllerBase
    {
        private readonly ISupportTicketManager _supportTicketManager;

        public SupportTicketsController(ISupportTicketManager supportTicketManager)
        {
            _supportTicketManager = supportTicketManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupportTicketCreateDto dto)
        {
            var userId = User.GetUserId();
            var result = await _supportTicketManager.CreateAsync(dto, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = User.GetUserId();
            var result = await _supportTicketManager.GetUserTicketsAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.GetUserId();
            var result = await _supportTicketManager.GetTicketByIdAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}