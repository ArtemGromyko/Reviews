

namespace Entities.RequestFeatures.Parameters
{
    public class ProductParameters : RequestParameters
    {
        public ProductParameters()
        {
            OrderBy = "name";
        }
        public string Categories { get; set; }
        public string Genres { get; set; }
        public string Countries { get; set; }
        public string SearchTerm { get; set; }
    }
}
