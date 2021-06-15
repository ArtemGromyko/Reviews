

namespace Entities.RequestFeatures
{
    public class ReviewParameters : RequestParameters
    {
        public uint MaxRaiting { get; set; } = 10;
        public uint MinRaiting { get; set; }
        public bool ValidRaitingRange => MaxRaiting > MinRaiting && MaxRaiting <= 10;
        public string SearchTerm { get; set; }
    }
}
