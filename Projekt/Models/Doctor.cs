using System.Collections.Generic;

namespace Projekt.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
