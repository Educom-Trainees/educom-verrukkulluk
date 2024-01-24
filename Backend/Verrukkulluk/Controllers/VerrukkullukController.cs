using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;

namespace Verrukkulluk.Controllers
{
    public class VerrukkullukController : Controller
    {
        public IActionResult Recept()
        {
            return View("Recipe");
        }

        public IActionResult MijnRecepten()
        {
            return View("MyRecipes");
        }

        public IActionResult Event(string eventName)
        {
            Event eventModel = GetEventData(eventName);
            return View(eventModel);
        }

        private Event GetEventData(string eventName)
        {
            switch (eventName?.ToLower())
            {
                default:
                    return new Event
                    {
                        Title = "Vegetarisch koken",
                        Description = "Een workshop vegetarisch koken, onder leiding van Trientje Hupsakee",
                        Date = new DateOnly(2024, 01, 30),
                        StartTime = new TimeOnly(14, 0),
                        EndTime = new TimeOnly(16, 0),
                        Place = "Jaarbeurs Utrecht"
                    };
                case "test1":
                    return new Event
                    {
                        Title = "Test1",
                        Description = "Test1 description",
                        Date = new DateOnly(2024, 01, 30),
                        StartTime = new TimeOnly(14, 0),
                        EndTime = new TimeOnly(16, 0),
                        Place = "Jaarbeurs Utrecht"
                    };
                case "test2":
                    return new Event
                    {
                        Title = "Test2",
                        Description = "Test2 description",
                        Date = new DateOnly(2024, 01, 30),
                        StartTime = new TimeOnly(14, 0),
                        EndTime = new TimeOnly(16, 0),
                        Place = "Jaarbeurs Utrecht"
                    };
                case "test3":
                    return new Event
                    {
                        Title = "Test3",
                        Description = "Test3 description",
                        Date = new DateOnly(2024, 01, 30),
                        StartTime = new TimeOnly(14, 0),
                        EndTime = new TimeOnly(16, 0),
                        Place = "Jaarbeurs Utrecht"
                    };
            }
        }

    }
}
