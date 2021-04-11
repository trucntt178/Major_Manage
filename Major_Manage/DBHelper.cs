using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Major_Manage
{
    class DBHelper
    {
        public static SqlConnection getConnection()
        {
            SqlConnection conn = new SqlConnection(@"uid=sa;pwd=lamtuandung;Initial Catalog=Majors;Data Source=MSI\SQLEXPRESS");

            return conn;
        }
    }
}
