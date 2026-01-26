using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Web.Controllers;

public class FoodController : Controller
{
    private readonly IFoodLookupService _food;
    public FoodController(IFoodLookupService food) => _food = food;

    public IActionResult Index() => View(new List<FoodSearchResult>());

    [HttpPost]
    public async Task<IActionResult> Index(string q)
    {
        var results = string.IsNullOrWhiteSpace(q)
            ? new List<FoodSearchResult>()
            : await _food.SearchFoodAsync(q);

        ViewBag.Query = q;
        return View(results);
    }
}
