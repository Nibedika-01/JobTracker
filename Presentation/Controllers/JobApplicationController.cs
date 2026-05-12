using JobTracker.Application.DTOs;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobTracker.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class JobApplicationController : ControllerBase
{
    private readonly IJobApplicationService _service;
    public JobApplicationController(IJobApplicationService service) => _service = service;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!; //helper func that reads userid from jwttoken

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] ApplicationStatus? status)

    {
        var result = await _service.GetAllAsync(UserId, status);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id, UserId);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateApplicationDto dto)
    {
        var result = await _service.CreateAsync(dto, UserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateApplicationDto dto)
    {
        var result = await _service.UpdateAsync(id, dto, UserId);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id, UserId);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _service.GetStatsAsync(UserId);
        return Ok(stats);
    }

}