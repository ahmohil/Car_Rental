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
        public IActionResult AddCar(Car car)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(car);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine("HERE");
            return View("Index");
        }

    }
}
