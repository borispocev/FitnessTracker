using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Data;
using FitnessTracker.Infrastructure.Repositories;

namespace FitnessTracker.Infrastructure.Services;

public class ExerciseService : EfRepository<Exercise>, IExerciseService
{
    public ExerciseService(AppDbContext db) : base(db) { }
}

