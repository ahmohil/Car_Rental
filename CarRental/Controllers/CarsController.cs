using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRental.Models;
using CarRental.Data;
using Microsoft.AspNetCore.Authorization;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly CarRentalDbContext _context;

        public CarController(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }
        [Authorize]
        public async Task<IActionResult> MyCars()
        {
            string userEmail = User.Identity.Name;

            var cars = await _context.Cars.Where(car => car.User == userEmail).ToListAsync();
            return View(cars);
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Car car)
        {
            car.User = User.Identity.Name;
            _context.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,Year,Price,Color,User,IsAvailable")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }
            try
            {
                car.User = User.Identity.Name;
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(car.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(MyCars));

        }

        // GET: Cars/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Rent(int carId)
        {
            var model = new RentViewModel { CarId = carId };
            return View(model);
        }

        [HttpPost]
        public IActionResult RentCar(RentViewModel model, DateTime RentDate, DateTime ReturnDate)
        {
            if(RentDate > ReturnDate)
            {
                return View("Rent", model);
            }
               
            if (ModelState.IsValid)
            {
                var rentedCar = _context.Cars.Find(model.CarId);

                int rentalDurationDays = (int)(ReturnDate - RentDate).TotalDays;
                decimal discountPercentage = 0;
                if (rentalDurationDays > 15)
                {
                    discountPercentage = 0.2m; // 20% discount
                }
                else if (rentalDurationDays > 7)
                {
                    discountPercentage = 0.1m; // 10% discount
                }

                int Amount = rentalDurationDays * rentedCar.Price;
                Amount = Convert.ToInt32(Amount * (1 - discountPercentage));

                var rental = new Rental
                {
                    RentalDate = RentDate,
                    ReturnDate = ReturnDate,
                    CarId = model.CarId,
                    Amount = Amount,
                    Renter = User.Identity.Name,
                    Owner = rentedCar.User
                };

                // Save the rental to the database
                _context.Rentals.Add(rental);
                _context.SaveChanges();


                if (rentedCar != null)
                {
                    rentedCar.IsAvailable = false;
                    _context.SaveChanges();
                }       

                // Redirect to a confirmation page or the Index action
                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return to the Rent view with validation errors
            return View("Rent", model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyCars));
        }

        public IActionResult SearchByMake()
        {
            return View();
        }
        public IActionResult SearchByModel()
        {
            return View();
        }
        public IActionResult SearchByYear()
        {
            return View();
        }
        public IActionResult SearchByColor()
        {
            return View();
        }
        public IActionResult SearchByPrice()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchByMake([Bind("Make,Model,Year,Color,User,IsAvailable")] Car car)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(car.Make))
            {
                query = query.Where(c => c.Make.Contains(car.Make));
            }

            var searchResults = await query.ToListAsync();
            return View("Index", searchResults);
        }
        [HttpPost]
        public async Task<IActionResult> SearchByModel([Bind("Make,Model,Year,Color,User,IsAvailable")] Car car)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(car.Model))
            {
                query = query.Where(c => c.Model.Contains(car.Model));
            }

            var searchResults = await query.ToListAsync();
            return View("Index", searchResults);
        }
        [HttpPost]
        public async Task<IActionResult> SearchByYear([Bind("Make,Model,Year,Color,User,IsAvailable")] Car car)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(car.Year.ToString()))
            {
                query = query.Where(c => c.Year.ToString().Contains(car.Year.ToString()));
            }

            var searchResults = await query.ToListAsync();
            return View("Index", searchResults);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByPrice(int low, int high)
        {
            var query = _context.Cars.AsQueryable();

            // Assuming you want to filter by price
            query = query.Where(c => c.Price >= low && c.Price <= high);

            var searchResults = await query.ToListAsync();
            return View("Index", searchResults);
        }
        [HttpPost]
        public async Task<IActionResult> SearchByColor([Bind("Make,Model,Year,Color,User ,IsAvailable")] Car car)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(car.Color))
            {
                query = query.Where(c => c.Color.Contains(car.Color));
            }

            var searchResults = await query.ToListAsync();
            return View("Index", searchResults);
        }



        public async Task<IActionResult> Available()
        {
            var availableCars = await _context.Cars.Where(c => c.IsAvailable).ToListAsync();
            return View("Index", availableCars);
        }

        

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
