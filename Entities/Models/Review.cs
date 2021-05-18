using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Review
    {
        [Column("ReviewId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Review is a reqired field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the heading is 30 characters.")]
        public string Heading { get; set; }

        [Required(ErrorMessage = "Text is a reqired field.")]
        [MaxLength(1000, ErrorMessage = "Maximum length for the text is 1000 characters.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Raiting is a required field.")]
        [Range(0, 10)]
        public int Raiting { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
