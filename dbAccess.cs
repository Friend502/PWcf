using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace WcfConnectPaysbuy
{
    public class dbAccess
    {
        public SqlConnection dbConnect()
        {

            //strConn = ConfigurationManager.AppSettings["dbConnection"].ToString();

            string strConn = WebConfigurationManager.AppSettings["dbConnection"];
            //strConn = WebConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            //string strConn = WebConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            //strConn = WebConfigurationManager.ConfigurationSettings.AppSettings["dbConnection"];
            SqlConnection con = new SqlConnection(strConn);
            return con;
        }
    }
}