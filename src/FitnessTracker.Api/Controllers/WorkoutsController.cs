using FitnessTracker.Application.Interfaces;
using FitnessTracker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController : ControllerBase
{
    private readonly IWorkoutService _workouts;

    public WorkoutsController(IWorkoutService workouts) => _workouts = workouts;

    [HttpGet]
    public Task<List<Workout>> GetAll() => _workouts.GetAllAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Workout>> Get(int id)
    {
        var w = await _workouts.GetByIdAsync(id);
        return w is null ? NotFound() : Ok(w);
    }

    [HttpPost]
    public Task<Workout> Create(Workout workout) => _workouts.AddAsync(workout);

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Workout workout)
    {
        if (id != workout.Id) return BadRequest("Id mismatch");
        await _workouts.UpdateAsync(workout);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _workouts.DeleteAsync(id);
        return NoContent();
    }

    // EXTRA ACTION: attach exercise to workout
    [HttpPost("{workoutId:int}/add-exercise")]
    public async Task<IActionResult> AddExercise(int workoutId, [FromBody] AddExerciseRequest req)
    {
        await _workouts.AddExerciseToWorkoutAsync(workoutId, req.ExerciseId, req.Sets, req.Reps);
        return NoContent();
    }

    public record AddExerciseRequest(int ExerciseId, int Sets, int Reps);
}
