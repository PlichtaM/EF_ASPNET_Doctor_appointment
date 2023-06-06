using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Required]
        public string PatientName { get; set; }

        public string Notes { get; set; }

        [Required]
        public int DoctorId { get; set; } 
    }
}
