using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FitnessTracker.Domain.Entities;

public class MealLog
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public DateTime Date { get; set; }

    public int Calories { get; set; }
    public string MealType { get; set; } = string.Empty; // Breakfast/Lunch/Dinner/Snack

    public User? User { get; set; }
}

