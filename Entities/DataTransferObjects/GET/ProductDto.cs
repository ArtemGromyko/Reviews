using System;

namespace Entities.DataTransferObjects.GET
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slogan { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Country { get; set; }
        public string Genre { get; set; }
        public string Category { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }
    }
}
