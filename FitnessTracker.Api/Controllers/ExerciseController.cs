using FitnessTracker.Application.Interfaces;
using FitnessTracker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _exercises;
    public ExercisesController(IExerciseService exercises) => _exercises = exercises;

    [HttpGet]
    public Task<List<Exercise>> GetAll() => _exercises.GetAllAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Exercise>> Get(int id)
    {
        var e = await _exercises.GetByIdAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public Task<Exercise> Create(Exercise ex) => _exercises.AddAsync(ex);

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Exercise ex)
    {
        if (id != ex.Id) return BadRequest("Id mismatch");
        await _exercises.UpdateAsync(ex);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _exercises.DeleteAsync(id);
        return NoContent();
    }
}
