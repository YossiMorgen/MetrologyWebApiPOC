using IgalDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MetrologyWebAPI.Controllers
{
    public class DefaultController : ApiController
    {

        public void Get(int id, string sValues)
        {
            //SqlParameter[] param = new SqlParameter[2];
            //param[0] = new SqlParameter("@id", id);
            //param[1] = new SqlParameter("@Values", sValues);
            //string sCon = ConfigurationManager.ConnectionStrings["IgalDB"].ConnectionString;
            //DataSet dsTest = SqlDAC.ExecuteDataset
            //    (sCon, CommandType.StoredProcedure, "uspSetResolutionRelations", param);
            //if(dsTest.Tables.Count == 0)
            //{
            //    throw new Exception("No data found");
            //}   

//            DataTable dataTable = dsTest.Tables[0];
//            DataTable dataTable1 = dsTest.Tables[1];
//
//            StringBuilder sql = new StringBuilder();
//            sql.AppendLine("SELECT * ");
//            sql.AppendLine($"FROM Resolution WHERE ResolutionID = {id.ToString()}");
//            DataTable dt = SqlDAC.ExecuteDataset
//                (sCon, CommandType.Text, sql.ToString(), param).Tables[0];

//            int Age = (int)SqlDAC.ExecuteScalar
//                (sCon, CommandType.Text, "SELECT Age FROM Person WHERE Name = @Name", 
//                new SqlParameter("@Name", "John"));
//            string s = "1";
//            int i = -9999999;
//            int.TryParse(s, out i);
        }
    }
}
