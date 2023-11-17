using CarRental.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        public CarController() { }

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
            return View("Index");
        }

    }
}
