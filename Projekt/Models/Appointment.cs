using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Required]
        public int DoctorId { get; set; }
       
        public Doctor Doctor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Slot { get; set; }

        [Required]
        public string PatientId { get; set; }
        public User Patient { get; set; }

        public string Remarks { get; set; }
    }
}
