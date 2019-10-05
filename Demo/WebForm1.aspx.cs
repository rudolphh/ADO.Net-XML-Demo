using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;

namespace Demo
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private readonly string SourceCS = ConfigurationManager.ConnectionStrings["SourceCS"].ConnectionString;
        private readonly string DestinationCS = ConfigurationManager.ConnectionStrings["DestinationCS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection sourceConn = new SqlConnection(SourceCS))
            {
                SqlCommand cmd = new SqlCommand("Select * from Departments", sourceConn);
                sourceConn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader()) { 

                    using (SqlConnection destinationConn = new SqlConnection(DestinationCS))
                    {
                        using (SqlBulkCopy bc = new SqlBulkCopy(destinationConn))
                        {
                            bc.DestinationTableName = "Departments";
                            destinationConn.Open();
                            bc.WriteToServer(reader);
                        }
                    }

                }

                cmd = new SqlCommand("Select * from Employees", sourceConn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    using (SqlConnection destinationConn = new SqlConnection(DestinationCS))
                    {
                        using (SqlBulkCopy bc = new SqlBulkCopy(destinationConn))
                        {
                            bc.DestinationTableName = "Employees";
                            destinationConn.Open();
                            bc.WriteToServer(reader);
                        }
                    }

                }
            }
        }
    }
}