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
    public class FacetAccessor : IFacetAccesor
    {
        public List<Facet> SelectFacets()
        {
            List<Facet> facets = new List<Facet>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_facets";

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
                        Facet facet = new Facet()
                        {
                            FacetID = reader.GetString(0),
                            Description = reader.GetString(1),
                            DateCreated = reader.GetDateTime(2),
                            DateUpdated = reader.GetDateTime(3),
                            Active = reader.GetBoolean(4),
                            RubricID = reader.GetInt32(5)
                        };
                        facets.Add(facet);
                    }
                }
                else
                {
                    throw new ApplicationException("No facets found");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return facets;
        }

        public List<Facet> SelectFacetsByRubricID(int rubricID)
        {
            List<Facet> facets = new List<Facet>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_facets_by_rubric_id";

            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RubricID", SqlDbType.Int);
            cmd.Parameters["@RubricID"].Value = rubricID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Facet facet = new Facet()
                        {
                            FacetID = reader.GetString(0),
                            Description = reader.GetString(1),
                            DateCreated = reader.GetDateTime(2),
                            DateUpdated = reader.GetDateTime(3),
                            Active = reader.GetBoolean(4),
                            RubricID = reader.GetInt32(5)
                        };
                        facets.Add(facet);
                    }
                }
                else
                {
                    throw new ApplicationException("No facets found");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return facets;
        }
    }
}
