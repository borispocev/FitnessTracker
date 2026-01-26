using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Interfaces
{
    public record FoodSearchResult(string Name, int KcalPer100g, string Brand, string ImageUrl);

    public interface IFoodLookupService
    {
        Task<List<FoodSearchResult>> SearchFoodAsync(string query, CancellationToken ct = default);
    }

}
