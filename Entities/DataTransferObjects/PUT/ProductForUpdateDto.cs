using Entities.DataTransferObjects.ManipulationDto;
using Entities.DataTransferObjects.POST;
using System.Collections.Generic;

namespace Entities.DataTransferObjects.PUT
{
    public class ProductForUpdateDto : ProductForManipulationDto
    {
        public IEnumerable<ReviewForCreationDto> Reviews { get; set; }
        public IEnumerable<PersonForCreationDto> Actors { get; set; }
        public IEnumerable<PersonForCreationDto> Directors { get; set; }
    }
}
