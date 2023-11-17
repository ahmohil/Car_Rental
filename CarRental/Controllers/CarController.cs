using CarRental.Context;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly AppDbContext _context;

        public CarController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddCar()
        {
            return View("add-car");
        }

        [HttpPost]
        public IActionResult AddCar(CarView car)
        {
            
            if (ModelState.IsValid)
            {

                Car obj = new()
                {
                    Make = car.Make,
                    Model = car.Model,
                    Variant = car.Variant,
                    DailyPrice = car.DailyPrice,
                    Color = car.Color
                };

                _context.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }

    }
}
