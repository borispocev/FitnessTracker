using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Application.Interfaces;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeightLogsController : ControllerBase
{
    private readonly IWeightLogService _weights;

    public WeightLogsController(IWeightLogService weights) => _weights = weights;

    [HttpGet]
    public Task<List<WeightLog>> GetAll() => _weights.GetAllAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WeightLog>> Get(int id)
    {
        var w = await _weights.GetByIdAsync(id);
        return w is null ? NotFound() : Ok(w);
    }

    [HttpPost]
    public Task<WeightLog> Create(WeightLog w)
    {
        w.Id = 0;
        return _weights.AddAsync(w);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, WeightLog w)
    {
        if (id != w.Id) return BadRequest("Id mismatch");
        await _weights.UpdateAsync(w);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _weights.DeleteAsync(id);
        return NoContent();
    }
}
