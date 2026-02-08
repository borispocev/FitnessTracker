using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Domain.Entities;

public class Workout
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public DateTime Date { get; set; }

    public User? User { get; set; }

    public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}

