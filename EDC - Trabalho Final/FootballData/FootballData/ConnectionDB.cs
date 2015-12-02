using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace FootballData
{
    public class ConnectionDB
    {
        private static SqlConnection con;
        static ConnectionDB()
        {
            string ConString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            con = new SqlConnection(ConString);
        }
        public static SqlConnection getConnection()
        {
            return con;
        }
    }
}