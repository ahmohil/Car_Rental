namespace CarRental.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }
        public int CarId { get; set; }
        public decimal RentPerDay { get; set; }
        public Car Car { get; set; }
    }
}
