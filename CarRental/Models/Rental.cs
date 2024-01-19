namespace CarRental.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int CarId { get; set; }
        public int Amount { get; set; }
        public string Renter { get; set; }
        public string Owner { get; set; }

        public Car Car { get; set; }
    }
}
