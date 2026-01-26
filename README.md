Fitness Tracking App

A full-stack fitness tracking web application built with ASP.NET Core (.NET 8) following **Onion Architecture** principles.

Architecture
- Domain
- Application
- Infrastructure
- API
- Web (MVC)

Features
- User management (CRUD)
- Workout tracking
- Exercise management
- Meal logging & calorie tracking
- Daily summary
- Weight progression & BMI visualization
- External API integration (OpenFoodFacts)
- Modern dashboard UI

Progress Tracking
- Weight history (WeightLogs)
- BMI calculation based on height and weight
- Chart.js visualization
- Daily workout & calorie summary

External API
- OpenFoodFacts API
- Food search with transformed data output

Technologies
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server (LocalDB)
- Chart.js
- Bootstrap 5
- Onion Architecture

**How to Run**
1. Open solution in Visual Studio 2022
2. Set startup projects:
   - FitnessTracker.Api
   - FitnessTracker.Web
3. Run migrations:
  -Update-Database -StartupProject FitnessTracker.Api
4. Press **F5**

Author
**Boris Pocev**  

