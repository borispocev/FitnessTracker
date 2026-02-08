using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly IFoodLookupService _food;
    public FoodController(IFoodLookupService food) => _food = food;

    // Transformed output: Name + kcal/100g + brand + image
    [HttpGet("search")]
    public Task<List<FoodSearchResult>> Search([FromQuery] string q)
        => _food.SearchFoodAsync(q);
}
