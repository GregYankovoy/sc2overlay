using Microsoft.AspNetCore.Mvc;
using Overlay.Data;

namespace Overlay.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SC2Controller : ControllerBase
{
    private readonly ILogger<SC2Controller> _logger;
    private readonly SC2Client _sc2;

    public SC2Controller(ILogger<SC2Controller> logger, SC2Client sc2)
    {
        _logger = logger;
        _sc2 = sc2;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _sc2.GetAllAsync());
    }

    [HttpGet, Route("game")]
    public async Task<IActionResult> GetGameAsync()
    {
        return Ok(await _sc2.GetGameAsync());
    }

    [HttpGet, Route("ui")]
    public async Task<IActionResult> GetUIAsync()
    {
        return Ok(await _sc2.GetUIAsync());
    }
}