using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class PlacesOfBirth
    {
        [Key]
        public int BirthPlaceId { get; set; }

        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
    }
}
