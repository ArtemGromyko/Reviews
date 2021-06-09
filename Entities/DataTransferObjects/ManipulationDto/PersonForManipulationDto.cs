using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.ManipulationDto
{
    public abstract class PersonForManipulationDto
    {
        [Required(ErrorMessage = "Person name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the name is 60 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Person birth date is a required field.")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Person birth place is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the birth place is 60 characters.")]
        public string BirthPlace { get; set; }
        [Range(0.0, 3.0, ErrorMessage = "Height is required field (0 - 3).")]
        public double Height { get; set; }
    }
}
