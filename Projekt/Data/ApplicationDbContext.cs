using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt.Models;
using System;

namespace Projekt.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Przykładowy użytkownik
            var appUser = new User
            {
                Id = "1",
                Email = "abc@abc.pl",
                EmailConfirmed = true,
                UserName = "abc@abc.pl",
                NormalizedUserName = "ABC@ABC.PL"
            };
            PasswordHasher<User> ph = new PasswordHasher<User>();
            appUser.PasswordHash = ph.HashPassword(appUser, "Haslo1!");

            // Przykładowe lekarze
            var doctor1 = new Doctor
            {
                Id = 1,
                Name = "Dr. Smith",
                Specialization = "Cardiology"
            };

            var doctor2 = new Doctor
            {
                Id = 2,
                Name = "Dr. Johnson",
                Specialization = "Dermatology"
            };

            modelBuilder.Entity<User>().HasData(appUser);
            modelBuilder.Entity<Doctor>().HasData(doctor1, doctor2);

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    Id = 1,
                    Specialization = "Cardiology",
                    DoctorId = 1,
                    Date = new DateTime(2023, 6, 12),
                    Slot = new DateTime(2023, 6, 12, 9, 0, 0),
                    Remarks = "Regular checkup",
                    PatientId = "1"
                },
                new Appointment
                {
                    Id = 2,
                    Specialization = "Dermatology",
                    DoctorId = 2,
                    Date = new DateTime(2023, 6, 12),
                    Slot = new DateTime(2023, 6, 12, 10, 0, 0),
                    Remarks = "Skin rash",
                    PatientId = "1"
                });

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
