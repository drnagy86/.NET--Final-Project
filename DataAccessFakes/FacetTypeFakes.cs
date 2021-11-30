using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;

namespace DataAccessFakes
{
    public class FacetTypeFakes : IFacetTypeAccessor
    {
        List<FacetType> _facetTypes = new List<FacetType>();

        public FacetTypeFakes()
        {
            _facetTypes.Add(new FacetType()
            {
                FacetTypeID = "Explaination",
                Description = "A long description",
                Active = true

            });

            _facetTypes.Add(new FacetType()
            {
                FacetTypeID = "Test",
                Description = "A long description",
                Active = true

            });

        }

        public List<FacetType> SelectFacetTypes()
        {
            return _facetTypes;

        }
    }
}
