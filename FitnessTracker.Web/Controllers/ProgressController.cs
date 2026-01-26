using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Application.Interfaces;

namespace FitnessTracker.Web.Controllers;

public class ProgressController : Controller
{
    private readonly IUserService _users;
    private readonly IProgressService _progress;
    private readonly IWeightLogService _weightLogs;

    public ProgressController(IUserService users, IProgressService progress, IWeightLogService weightLogs)
    {
        _users = users;
        _progress = progress;
        _weightLogs = weightLogs;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Users = await _users.GetAllAsync();
        return View();
    }

    public async Task<IActionResult> Chart(int userId)
    {
        var points = await _progress.GetWeightProgressAsync(userId);
        ViewBag.UserId = userId;
        return View(points);
    }

    [HttpPost]
    public async Task<IActionResult> LogWeight(int userId, DateTime date, double weight)
    {
        /*await _weightLogs.AddAsync(new WeightLog { UserId = userId, Date = date, Weight = weight });
        return RedirectToAction(nameof(Chart), new { userId });*/

       
        await _weightLogs.AddAsync(new WeightLog
        {
            UserId = userId,
            Date = date,
            Weight = weight
        });

       
        var user = await _users.GetByIdAsync(userId);
        if (user != null)
        {
            user.Weight = weight;
            await _users.UpdateAsync(user);
        }

        return RedirectToAction(nameof(Chart), new { userId });
    }
}

