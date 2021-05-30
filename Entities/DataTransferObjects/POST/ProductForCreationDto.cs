using System;
using System.Collections.Generic;

namespace Entities.DataTransferObjects.POST
{
    public class ProductForCreationDto
    {
        public string Name { get; set; }
        public string Slogan { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Country { get; set; }
        public string Genre { get; set; }
        public string Category { get; set; }
        public IEnumerable<ReviewForCreationDto> Reviews { get; set; }
        public IEnumerable<PersonForCreationDto> Actors { get; set; }
        public IEnumerable<PersonForCreationDto> Directors { get; set; }
    }
}
