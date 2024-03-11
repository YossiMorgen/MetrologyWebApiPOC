using IgalDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MetrologyWebAPI
{
    public class MetroDAL: clsBaseConnection
    {
        public MetroDAL() : base()
        {
            ConnectionString = "Data Source=IGAL-PC;Initial Catalog=IgalDB;Integrated Security=True";
        }

        public MetroDAL(string ConnectionString) : base(ConnectionString)
        {
        }

        public DataSet SetResolutionRelations(int id, string sValues)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@id", id);
            param[1] = new SqlParameter("@Values", sValues);
            return ExecuteDataset(CommandType.StoredProcedure, "uspSetResolutionRelations", param);
        }   
    }
}