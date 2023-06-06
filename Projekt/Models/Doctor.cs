using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Specialization { get; set; }
    }
}
