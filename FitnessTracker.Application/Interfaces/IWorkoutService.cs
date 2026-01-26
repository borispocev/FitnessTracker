using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Application.Interfaces;

public interface IWorkoutService : IRepository<Workout>
{
    Task AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int sets, int reps, CancellationToken ct = default);
    Task<List<Workout>> GetAllWithUserAndExercisesAsync(CancellationToken ct = default);

}

