namespace CarRental.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
        public string User { get; set; }

        public List<Rental> Rentals { get; set; }
    }
}
