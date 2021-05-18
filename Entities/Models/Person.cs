using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{

    public class Person
    {
        [Column("PersonId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Person name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the name is 60 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Person birth date is a required field.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Person birth place is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the birth place is 60 characters.")]
        public string BirthPlace { get; set; }

        [Range(0.0, 3.0)]
        public double Height { get; set; }

        public IEnumerable<Product> ProductsActor { get; set; }

        public IEnumerable<Product> ProductsDirector { get; set; }
    }
}
