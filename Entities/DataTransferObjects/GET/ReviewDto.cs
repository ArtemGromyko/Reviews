using System;

namespace Entities.DataTransferObjects.GET
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string Text { get; set; }
        public int Raiting { get; set; }
    }
}
