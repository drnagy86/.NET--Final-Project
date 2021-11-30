using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessFakes;
using DataAccessLayer;


namespace LogicLayer
{
    public class FacetTypeManager : IFacetTypeManager
    {

        IFacetTypeAccessor _facetTypeAccessor = null;

        public FacetTypeManager()
        {
            _facetTypeAccessor = new FacetTypeAccessor();
        }

        public FacetTypeManager(IFacetTypeAccessor facetTypeAccessor)
        {
            _facetTypeAccessor = facetTypeAccessor;
        }

        public List<FacetType> RetrieveFacetTypes()
        {
            List<FacetType> facetTypes = new List<FacetType>();

            try
            {
                facetTypes = _facetTypeAccessor.SelectFacetTypes();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            if (facetTypes == null)
            {
                throw new ApplicationException("No facet types retrieved.");
            }

            return facetTypes;
        }
    }
}
