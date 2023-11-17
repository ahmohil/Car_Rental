using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarRental.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Variant { get; set; }
        public string Color { get; set; }
        public decimal DailyPrice { get; set; }

        public ICollection<Rental> Rentals { get; set; }
        public ICollection<Post> Posts { get; set; }

    }

}
