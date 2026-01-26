using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public string Goals { get; set; } = string.Empty;

    public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    public ICollection<MealLog> MealLogs { get; set; } = new List<MealLog>();
    public ICollection<WeightLog> WeightLogs { get; set; } = new List<WeightLog>();

}
