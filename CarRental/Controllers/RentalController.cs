using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRental.Data;

namespace CarRental.Controllers
{
    public class RentalController : Controller
    {
        private readonly CarRentalDbContext _context;

        public RentalController(CarRentalDbContext context)
        {
            _context = context;
        }

        // GET: Rentals
        // Return All Rentals where car is mine
        public async Task<IActionResult> Index()
        {

            var rentals = await _context.Rentals
                .Include(r => r.Car) // Eagerly load the associated Car entity
                .ToListAsync();

            return View(rentals);
        }

        // All Rentals where I took someone's car
        [Authorize]
        public async Task<IActionResult> MyRentals()
        {
            var rentals = await _context.Rentals
                .Where(r => r.Renter == User.Identity.Name || r.Owner == User.Identity.Name)          
                .Include(r => r.Car) // Eagerly load the associated Car entity
                .ToListAsync();


            return View(rentals);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }
    }
}
