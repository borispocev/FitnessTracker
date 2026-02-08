using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Interfaces
{
    public record DailySummaryDto(
        int UserId,
        DateTime Date,
        int TotalCalories,
        int WorkoutCount,
        List<string> ExercisesDone
    );

    public record WeightProgressPoint(DateTime Date, double Weight, double BMI);

    public interface IProgressService
    {
        Task<DailySummaryDto> GetDailySummaryAsync(int userId, DateTime date, CancellationToken ct = default);
        Task<List<WeightProgressPoint>> GetWeightProgressAsync(int userId, CancellationToken ct = default);

    }

}
