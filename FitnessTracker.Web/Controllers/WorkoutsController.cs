using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Data;
using FitnessTracker.Application.Interfaces;

namespace FitnessTracker.Web.Controllers;

public class WorkoutsController : Controller
{
    private readonly IWorkoutService _workouts;
    private readonly IUserService _users;
    private readonly IExerciseService _exercises;

    public WorkoutsController(IWorkoutService workouts, IUserService users, IExerciseService exercises)
    {
        _workouts = workouts;
        _users = users;
        _exercises = exercises;
    }

    private async Task LoadUsersAsync(int? selectedUserId = null)
    {
        var users = await _users.GetAllAsync();
        ViewBag.UserSelectList = new SelectList(users, "Id", "Name", selectedUserId);
    }

    public async Task<IActionResult> Index()
    {
        var workouts = await _workouts.GetAllWithUserAndExercisesAsync();
        return View(workouts);

    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadUsersAsync();
        return View(new Workout { Date = DateTime.Now });
    }

    [HttpPost]
    public async Task<IActionResult> Create(Workout workout)
    {
        
        if (!ModelState.IsValid)
        {
            await LoadUsersAsync(workout.UserId);
            return View(workout);
        }

        // Ensure identity insert not triggered
        workout.Id = 0;

        await _workouts.AddAsync(workout);
        return RedirectToAction(nameof(Index));
    }

    // Add exercise form
    [HttpGet]
    public async Task<IActionResult> AddExercise(int workoutId)
    {
        var exercises = await _exercises.GetAllAsync();
        ViewBag.WorkoutId = workoutId;
        ViewBag.ExerciseSelectList = new SelectList(exercises, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddExercise(int workoutId, int exerciseId, int sets, int reps)
    {
        await _workouts.AddExerciseToWorkoutAsync(workoutId, exerciseId, sets, reps);
        return RedirectToAction(nameof(Index));
    }
   
}
