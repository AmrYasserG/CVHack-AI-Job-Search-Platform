using CVHack.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API
{
    [Route("api/admin/tickets")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminSupportTicketsController : ControllerBase
    {
        private readonly ISupportTicketManager _supportTicketManager;

        public AdminSupportTicketsController(ISupportTicketManager supportTicketManager)
        {
            _supportTicketManager = supportTicketManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supportTicketManager.GetAllTicketsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _supportTicketManager.GetTicketDetailsAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateTicketStatusDto dto)
        {
            var result = await _supportTicketManager.UpdateTicketStatusAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }
    }
}