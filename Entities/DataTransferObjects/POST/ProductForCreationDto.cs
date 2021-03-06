using Entities.DataTransferObjects.ManipulationDto;
using System.Collections.Generic;

namespace Entities.DataTransferObjects.POST
{
    public class ProductForCreationDto : ProductForManipulationDto
    {
        public IEnumerable<ReviewForCreationDto> Reviews { get; set; }
        public IEnumerable<PersonForCreationDto> Actors { get; set; }
        public IEnumerable<PersonForCreationDto> Directors { get; set; }
    }
}
