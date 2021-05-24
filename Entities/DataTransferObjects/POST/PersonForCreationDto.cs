﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.POST
{
    public class PersonForCreationDto
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public double Height { get; set; }
    }
}
