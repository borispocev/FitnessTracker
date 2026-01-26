using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Data;
using FitnessTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infrastructure.Services;

public class WorkoutService : EfRepository<Workout>, IWorkoutService
{
    public WorkoutService(AppDbContext db) : base(db) { }

    public async Task<List<Workout>> GetAllWithDetailsAsync(CancellationToken ct = default)
    {
        return await _db.Workouts
            .AsNoTracking()
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .ToListAsync(ct);
    }

    public async Task<List<Workout>> GetAllWithUserAndExercisesAsync(CancellationToken ct = default)
    {
        return await _db.Workouts
            .AsNoTracking()
            .Include(w => w.User)
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
            .OrderByDescending(w => w.Date)
            .ToListAsync(ct);
    }

    public async Task AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int sets, int reps, CancellationToken ct = default)
    {
        var workout = await _db.Workouts.FindAsync([workoutId], ct);
        if (workout is null) throw new InvalidOperationException("Workout not found.");

        var exercise = await _db.Exercises.FindAsync([exerciseId], ct);
        if (exercise is null) throw new InvalidOperationException("Exercise not found.");

        var existing = await _db.WorkoutExercises.FindAsync([workoutId, exerciseId], ct);
        if (existing is null)
        {
            _db.WorkoutExercises.Add(new WorkoutExercise
            {
                WorkoutId = workoutId,
                ExerciseId = exerciseId,
                Sets = sets,
                Reps = reps
            });
        }
        else
        {
            existing.Sets = sets;
            existing.Reps = reps;
            _db.WorkoutExercises.Update(existing);
        }

        await _db.SaveChangesAsync(ct);
    }
}

