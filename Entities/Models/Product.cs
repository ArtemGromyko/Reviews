using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Product
    {
        [Column("ProductId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Product name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the name is 30 characters.")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Maximum length for the slogan is 60 characters.")]
        public string Slogan { get; set; }

        [Required(ErrorMessage = "Product release date is a required field.")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Product country is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the country is 20 characters.")]
        public string Country { get; set; }
        
        [Required(ErrorMessage = "Product genre is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the genre is 30 characters.")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Product category is a required field.")]
        [MaxLength(15, ErrorMessage = "Maximum length for the category is 20 characters.")]
        public string Category { get; set; }

        //public double Raiting { get; } 

        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<Person> Actors { get; set; }
        public IEnumerable<Person> Directors { get; set; }
    }
}
