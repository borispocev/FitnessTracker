using FitnessTracker.Application.Interfaces;
using FitnessTracker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Web.Controllers;


public class HomeController : Controller
{
    private readonly IUserService _users;
    private readonly IWorkoutService _workouts;
    private readonly IMealLogService _mealLogs;
    private readonly IProgressService _progress;

    public HomeController(
        IUserService users,
        IWorkoutService workouts,
        IMealLogService mealLogs,
        IProgressService progress)
    {
        _users = users;
        _workouts = workouts;
        _mealLogs = mealLogs;
        _progress = progress;
    }
    private static (string Label, string PillClass) GetBmiStatus(double? bmi)
    {
        if (bmi is null || bmi <= 0)
            return ("BMI: —", "text-bg-secondary");

        // Underweight
        if (bmi < 18.5)
            return ($"BMI: {bmi:0.0} • Underweight", "text-bg-danger");

        // Normal/Optimal
        if (bmi < 25.0)
            return ($"BMI: {bmi:0.0} • Optimal", "text-bg-success");

        // Overweight (and obese)
        return ($"BMI: {bmi:0.0} • Overweight", "text-bg-danger");
    }

    public async Task<IActionResult> Index(int? userId)
    {
        var allUsers = await _users.GetAllAsync();
        var user = userId.HasValue
            ? allUsers.FirstOrDefault(u => u.Id == userId.Value)
            : allUsers.FirstOrDefault();

        ViewBag.AllUsers = allUsers;

        if (user is null)
            return View(new DashboardVm());

        var today = DateTime.Now.Date;

        // Recent workouts (latest 3 for user)
        // If you added GetAllWithUserAndExercisesAsync(), use it here.
        var workouts = await _workouts.GetAllAsync();
        var recentWorkouts = workouts
            .Where(w => w.UserId == user.Id)
            .OrderByDescending(w => w.Date)
            .Take(3)
            .ToList();

        // Today meals for user
        var meals = await _mealLogs.GetAllAsync();
        var todayMeals = meals
            .Where(m => m.UserId == user.Id && m.Date.Date == today)
            .OrderBy(m => m.Date)
            .ToList();

        var caloriesByMeal = todayMeals
            .GroupBy(m => string.IsNullOrWhiteSpace(m.MealType) ? "Other" : m.MealType.Trim())
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Calories));

        // Today summary (exercises list + counts)
        var summary = await _progress.GetDailySummaryAsync(user.Id, today);

        // Weight + BMI series (already implemented in your service)
        var weightSeries = await _progress.GetWeightProgressAsync(user.Id);
        double? currentBmi = weightSeries.Count > 0 ? weightSeries.Last().BMI : null;

        (string label, string pillClass) = GetBmiStatus(currentBmi);

        var vm = new DashboardVm
        {
            User = user,
            HeightDisplay = user.Height > 0 ? $"{user.Height:0.#} cm" : "—",
            RecentWorkouts = recentWorkouts,
            TodayMeals = todayMeals,
            TodayCalories = summary.TotalCalories,
            TodayWorkoutCount = summary.WorkoutCount,
            TodayExercises = summary.ExercisesDone,
            WeightSeries = weightSeries,
            CurrentBmi = currentBmi,
            BmiLabel = label,
            BmiPillClass = pillClass,
            CaloriesByMealType = caloriesByMeal,
            Goals = ParseGoals(user.Goals)
        };

        return View(vm);
    }

    private static List<(string Goal, int ProgressPct, string StatusText)> ParseGoals(string goalsText)
    {
        if (string.IsNullOrWhiteSpace(goalsText))
            return new();

        var parts = goalsText.Split(new[] { ';', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(x => x.Trim())
                             .Where(x => x.Length > 0)
                             .Take(3)
                             .ToList();

        // UI-only progress bars (since goals are free-text)
        var rnd = new Random(1);
        return parts.Select(g =>
        {
            var pct = rnd.Next(35, 85);
            var status = pct >= 80 ? "Almost there" : "In progress";
            return (g, pct, status);
        }).ToList();
    }
}
