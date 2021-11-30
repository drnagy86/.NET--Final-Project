using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public class FacetTypeAccessor : IFacetTypeAccessor
    {
        public List<FacetType> SelectFacetTypes()
        {
            List<FacetType> facetTypes = new List<FacetType>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_facet_types";

            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FacetType facetType = new FacetType()
                        {
                            FacetTypeID = reader.GetString(0),
                            Description = reader.GetString(1)
                        };
                        facetTypes.Add(facetType);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return facetTypes;
        }
    }
}
