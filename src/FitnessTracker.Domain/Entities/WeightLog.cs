using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Domain.Entities;

public class WeightLog
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public DateTime Date { get; set; }

    public double Weight { get; set; }

    public User? User { get; set; }
}
