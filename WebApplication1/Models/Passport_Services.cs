using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class PassportServices
    {
        [Key]
        public int ServiceId { get; set; }
        [Display(Name ="Service Type")]
        public string? ServiceType { get; set; }
        public float ServicePrice { get; set; }
        public int ExecutionDate { get; set; }
    }
}
