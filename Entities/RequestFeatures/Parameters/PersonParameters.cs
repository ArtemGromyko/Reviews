

using System;

namespace Entities.RequestFeatures.Parameters
{
    public class PersonParameters : RequestParameters
    {
        public PersonParameters()
        {
            OrderBy = "name";
        }
        public DateTime MinBirthDate { get; set; } = DateTime.MinValue;
        public DateTime MaxBirthDate { get; set; } = DateTime.MaxValue;
        public string BirthPlace { get; set; }
        public double MinHeight { get; set; } = 0.0;
        public double MaxHeight { get; set; } = 3.0;
        public string SearchTerm { get; set; }

        public bool ValidParametersRange => MinBirthDate < MaxBirthDate && MinHeight < MaxHeight;
    }
}
