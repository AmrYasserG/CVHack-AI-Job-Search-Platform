using CVHack.API;
using CVHack.BLL.Services.CvGenerator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cv")]
[Authorize]
public class CvGeneratorController : ControllerBase
{
    private readonly ICvGeneratorService _cvService;

    public CvGeneratorController(ICvGeneratorService cvService)
    {
        _cvService = cvService;
    }

    [HttpGet("generate/{jobId}")]
    public async Task<IActionResult> Generate(int jobId)
    {
        var userId = User.GetUserId();
        var result = await _cvService.GenerateAsync(userId, jobId);
        return Ok(result);
    }
}