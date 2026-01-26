using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progress;
    public ProgressController(IProgressService progress) => _progress = progress;

    [HttpGet("{userId:int}/daily-summary")]
    public Task<DailySummaryDto> DailySummary(int userId, [FromQuery] DateTime date)
        => _progress.GetDailySummaryAsync(userId, date);

    [HttpGet("{userId:int}/weight-series")]
    public Task<List<WeightProgressPoint>> WeightSeries(int userId)
        => _progress.GetWeightProgressAsync(userId);
}
