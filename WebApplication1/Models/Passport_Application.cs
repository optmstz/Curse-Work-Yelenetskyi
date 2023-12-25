    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace WebApplication1.Models
    {
        public class PassportApplication
        {
            public int PassportApplicationId { get; set; }
            [Display(Name ="Date of Submission")]
            public DateTime? DateOfSubmission { get; set; }
            [Display(Name ="Date of Execution")]
            public DateTime DateOfExecution { get; set; }

            public int? EmployeesId { get; set; }
            public Employees? Employees { get; set; }

            public string? ApplicationStatus { get; set; }
            public int ServiceId { get; set; }
            public PassportServices? PassportServices { get; set; }

            public int PersonPassportId { get; set; }
            public PersonPassports? PersonPassport { get; set; }
        }

        public class PassportApplicationConfiguration : IEntityTypeConfiguration<PassportApplication>
        {
            public void Configure(EntityTypeBuilder<PassportApplication> builder)
            {
                builder.HasKey(p => p.PassportApplicationId);

                builder.HasOne(p => p.Employees)
                       .WithMany()
                       .HasForeignKey(p => p.EmployeesId);

                builder.HasOne(p => p.PassportServices)
                       .WithMany()
                       .HasForeignKey(p => p.ServiceId);

                builder.HasOne(p => p.PersonPassport)
                    .WithMany()
                    .HasForeignKey(p => p.PersonPassportId)
                    .OnDelete(DeleteBehavior.Restrict);

            }
        }

    }
