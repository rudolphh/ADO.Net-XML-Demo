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
        string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(Server.MapPath("~/Data.xml"));

                DataTable dtDept = ds.Tables["Department"];
                DataTable dtEmp = ds.Tables["Employee"];

                conn.Open();

                using (SqlBulkCopy bc = new SqlBulkCopy(conn))
                {
                    bc.DestinationTableName = "departments";
                    bc.ColumnMappings.Add("ID", "ID");
                    bc.ColumnMappings.Add("Name", "Name");
                    bc.ColumnMappings.Add("Location", "Location");
                    bc.WriteToServer(dtDept);
                }

                using (SqlBulkCopy bc = new SqlBulkCopy(conn))
                {
                    bc.DestinationTableName = "Employees";
                    bc.ColumnMappings.Add("ID", "ID");
                    bc.ColumnMappings.Add("Name", "Name");
                    bc.ColumnMappings.Add("Gender", "Gender");
                    bc.ColumnMappings.Add("DepartmentId", "DepartmentId");
                    bc.WriteToServer(dtEmp);
                }
            }
        }
    }
}