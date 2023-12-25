using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RegistrationPlaces
    {
        [Key]
        public int RegistrationPlaceId { get; set; }

        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
    }

}
