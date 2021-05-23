using System;

namespace Entities.DataTransferObjects.GET
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public double Height { get; set; }
    }
}
