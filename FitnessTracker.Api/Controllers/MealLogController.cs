using FitnessTracker.Application.Interfaces;
using FitnessTracker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealLogsController : ControllerBase
{
    private readonly IMealLogService _meals;
    public MealLogsController(IMealLogService meals) => _meals = meals;

    [HttpGet]
    public Task<List<MealLog>> GetAll() => _meals.GetAllAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MealLog>> Get(int id)
    {
        var m = await _meals.GetByIdAsync(id);
        return m is null ? NotFound() : Ok(m);
    }

    [HttpPost]
    public Task<MealLog> Create(MealLog meal) => _meals.AddAsync(meal);

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MealLog meal)
    {
        if (id != meal.Id) return BadRequest("Id mismatch");
        await _meals.UpdateAsync(meal);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _meals.DeleteAsync(id);
        return NoContent();
    }
}
