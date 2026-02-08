using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Domain.Entities;

public class WorkoutExercise
{
    public int WorkoutId { get; set; }
    public Workout? Workout { get; set; }

    public int ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
}
