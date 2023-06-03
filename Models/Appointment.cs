using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Doctor specialization is required.")]
        public string DoctorSpecialization { get; set; }

        public bool IsAvailable { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
