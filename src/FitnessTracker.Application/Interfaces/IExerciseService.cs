using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Application.Interfaces;

public interface IExerciseService : IRepository<Exercise> { }

