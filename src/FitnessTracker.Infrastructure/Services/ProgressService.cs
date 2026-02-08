using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infrastructure.Services;

public class ProgressService : IProgressService
{
    private readonly AppDbContext _db;

    public ProgressService(AppDbContext db) => _db = db;

    public async Task<DailySummaryDto> GetDailySummaryAsync(int userId, DateTime date, CancellationToken ct = default)
    {
        var day = date.Date;

        var calories = await _db.MealLogs
            .Where(m => m.UserId == userId && m.Date.Date == day)
            .SumAsync(m => (int?)m.Calories, ct) ?? 0;

        var workouts = await _db.Workouts
            .Where(w => w.UserId == userId && w.Date.Date == day)
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
            .ToListAsync(ct);

        var exercises = workouts
            .SelectMany(w => w.WorkoutExercises)
            .Select(we => we.Exercise!.Name)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        return new DailySummaryDto(userId, day, calories, workouts.Count, exercises);
    }

    /*public async Task<List<WeightProgressPoint>> GetWeightProgressAsync(int userId, CancellationToken ct = default)
    {
        return await _db.WeightLogs
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .OrderBy(w => w.Date)
            .Select(w => new WeightProgressPoint(w.Date.Date, w.Weight))
            .ToListAsync(ct);
    }*/
    public async Task<List<WeightProgressPoint>> GetWeightProgressAsync(int userId, CancellationToken ct = default)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user is null)
            return new List<WeightProgressPoint>();

        if (user.Height <= 0)
        {
            // Height not set yet → BMI can't be computed
            return await _db.WeightLogs
                .AsNoTracking()
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.Date)
                .Select(w => new WeightProgressPoint(w.Date.Date, w.Weight, 0))
                .ToListAsync(ct);
        }

        double heightMeters = user.Height / 100.0;
        double denom = heightMeters * heightMeters;

        return await _db.WeightLogs
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .OrderBy(w => w.Date)
            .Select(w => new WeightProgressPoint(
                w.Date.Date,
                w.Weight,
                Math.Round(w.Weight / denom, 2)
            ))
            .ToListAsync(ct);
    }

}

