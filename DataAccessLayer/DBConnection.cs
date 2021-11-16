using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class DBConnection
    {
        public static SqlConnection GetConnection()
        {

            string connectionString = @"Data Source = localhost; Initial Catalog = rubric_db; Integrated Security = True";
            var connection = new SqlConnection(connectionString);

            return connection;

        }

    }
}
