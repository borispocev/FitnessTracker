using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using FitnessTracker.Application.Interfaces;

namespace FitnessTracker.Infrastructure.External;

public class OpenFoodFactsService : IFoodLookupService
{
    private readonly HttpClient _http;

    public OpenFoodFactsService(HttpClient http) => _http = http;

    public async Task<List<FoodSearchResult>> SearchFoodAsync(string query, CancellationToken ct = default)
    {
        try
        {
            var url =
                $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={Uri.EscapeDataString(query)}&search_simple=1&action=process&json=1&page_size=10";

            using var resp = await _http.GetAsync(url, ct);

            if (!resp.IsSuccessStatusCode)
                return new List<FoodSearchResult>();

            using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            var products = doc.RootElement.GetProperty("products");
            var results = new List<FoodSearchResult>();

            foreach (var p in products.EnumerateArray())
            {
                string name = p.TryGetProperty("product_name", out var pn) ? pn.GetString() ?? "" : "";
                string brand = p.TryGetProperty("brands", out var br) ? br.GetString() ?? "" : "";
                string image = p.TryGetProperty("image_front_small_url", out var img) ? img.GetString() ?? "" : "";

                int kcal = 0;
                if (p.TryGetProperty("nutriments", out var nut) &&
                    nut.TryGetProperty("energy-kcal_100g", out var ek))
                {
                    if (ek.ValueKind == JsonValueKind.Number)
                        kcal = (int)Math.Round(ek.GetDouble());
                }

                if (!string.IsNullOrWhiteSpace(name))
                    results.Add(new FoodSearchResult(name, kcal, brand, image));
            }

            return results.OrderByDescending(r => r.KcalPer100g).ToList();
        }
        catch (TaskCanceledException)
        {
            // timeout or cancellation
            return new List<FoodSearchResult>();
        }
        catch (HttpRequestException)
        {
            return new List<FoodSearchResult>();
        }
    }

}

