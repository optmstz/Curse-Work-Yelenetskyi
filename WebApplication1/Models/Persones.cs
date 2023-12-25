using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Name s reuired")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string PersonSurname { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly PersonDateOfBirth { get; set; }

        [Required(ErrorMessage = "Sex is required.")]
        public string PersonSex { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        public string PersonNationality { get; set; }

        [Required(ErrorMessage = "RNK is required.")]
        public int RNKNumber { get; set; }

        public int? PersonPassportId { get; set; }
        public PersonPassports? PersonPassport { get; set; }

        public int RegistrationPlaceId { get; set; }
        public RegistrationPlaces? RegistrationPlace { get; set; }

        public int BirthPlaceId { get; set; }
        public PlacesOfBirth? BirthPlace { get; set; }

    }


    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasOne(p => p.PersonPassport)
             .WithOne()
             .HasForeignKey<Person>(p => p.PersonPassportId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.RegistrationPlace)
                  .WithMany() 
                  .HasForeignKey(p => p.RegistrationPlaceId);

            builder.HasOne(p => p.BirthPlace)
                   .WithMany()
                   .HasForeignKey(p => p.BirthPlaceId);

            builder.HasIndex(p => p.RNKNumber).IsUnique();
        }
    }

}
