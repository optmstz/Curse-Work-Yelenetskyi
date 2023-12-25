using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class PersonPassports
    {
        public int? PersonPassportId { get; set; }
        [Display(Name = "Passport Number")]
        [Required(ErrorMessage = "Passport Number is required.")]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "Passport Number must be 9 digits.")]
        public int? Passport_Number { get; set; }
        public string? Authority { get; set; }
        public DateOnly? Date_of_Issue { get; set; }
        public DateOnly? Date_of_Expiry { get; set; }
        public int? PersonId { get; set; }
        public Person? Person { get; set; }
    }

    public class PersonPassportsConfiguration : IEntityTypeConfiguration<PersonPassports>
    {
        public void Configure(EntityTypeBuilder<PersonPassports> builder)
        {
            builder.HasKey(pp => pp.PersonPassportId);
            builder.HasIndex(pp => pp.Passport_Number).IsUnique();

            builder.HasOne(pp => pp.Person)
                .WithOne(p => p!.PersonPassport)
                .HasForeignKey<PersonPassports>(pp => pp.PersonId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }


}

