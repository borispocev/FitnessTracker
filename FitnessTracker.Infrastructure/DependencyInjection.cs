using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Infrastructure.Data;
using FitnessTracker.Infrastructure.External;
using FitnessTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IMealLogService, MealLogService>();
        services.AddScoped<IProgressService, ProgressService>();
        services.AddScoped<IWeightLogService, WeightLogService>();


        services.AddHttpClient<IFoodLookupService, OpenFoodFactsService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(15);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("FitnessTrackerApp/1.0");
        }).SetHandlerLifetime(TimeSpan.FromMinutes(5));

        
        services.AddHttpClient<IFoodLookupService, OpenFoodFactsService>();

        return services;
    }
}

