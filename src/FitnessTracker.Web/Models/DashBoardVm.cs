using FitnessTracker.Application.Interfaces;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Web.Models;

public class DashboardVm
{
    public User? User { get; set; }
    public double? CurrentBmi { get; set; }
    public string BmiLabel { get; set; } = "BMI: —";
    public string BmiPillClass { get; set; } = "text-bg-secondary";


    // “Top strip” info
    public string? HeightDisplay { get; set; } // e.g. "175 cm" or "—"

    // Cards
    public List<Workout> RecentWorkouts { get; set; } = new();
    public List<MealLog> TodayMeals { get; set; } = new();

    // Extra summary (Today Summary)
    public int TodayCalories { get; set; }
    public int TodayWorkoutCount { get; set; }
    public List<string> TodayExercises { get; set; } = new();

    // Charts

    public List<WeightProgressPoint> WeightSeries { get; set; } = new();

    public Dictionary<string, int> CaloriesByMealType { get; set; } = new();

    // “Goals” card: we’ll parse simple goals text (optional)
    public List<(string Goal, int ProgressPct, string StatusText)> Goals { get; set; } = new();
}
