using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<MealLog> MealLogs => Set<MealLog>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<WeightLog> WeightLogs => Set<WeightLog>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.Entity<WorkoutExercise>()
            .HasKey(we => new { we.WorkoutId, we.ExerciseId });

        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(we => we.Workout)
            .WithMany(w => w.WorkoutExercises)
            .HasForeignKey(we => we.WorkoutId);

        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(we => we.Exercise)
            .WithMany(e => e.WorkoutExercises)
            .HasForeignKey(we => we.ExerciseId);

        
        modelBuilder.Entity<Workout>()
            .HasOne(w => w.User)
            .WithMany(u => u.Workouts)
            .HasForeignKey(w => w.UserId);

        modelBuilder.Entity<MealLog>()
            .HasOne(m => m.User)
            .WithMany(u => u.MealLogs)
            .HasForeignKey(m => m.UserId);

        modelBuilder.Entity<WeightLog>()
            .HasOne(w => w.User)
            .WithMany(u => u.WeightLogs)
            .HasForeignKey(w => w.UserId);

        base.OnModelCreating(modelBuilder);
    }
}

