using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MetrologyWebAPI.Data
{
    public class DAL
    {
        public static class Validators
        {
            private static Regex digitsOnly = new Regex("^[\\d]+$", RegexOptions.IgnoreCase);
            private static Regex digitsDash = new Regex("^[\\d\\-\\s]+$", RegexOptions.IgnoreCase);
            private static Regex lettersOnly = new Regex("^[a-zA-Z\\sא-ת]+$", RegexOptions.IgnoreCase);
            private static Regex lettersEngNoSpace = new Regex("^[a-zA-Z]+$", RegexOptions.IgnoreCase);
            private static Regex lettersQuoteDash = new Regex("^[a-zA-Z\\\"\\-\\sא-ת]+$", RegexOptions.IgnoreCase);
            private static Regex lettersQuoteDigits = new Regex("^[א-ת\\\"\\d\\sa-zA-Z]+$", RegexOptions.IgnoreCase);
            private static Regex lettersEngDigits = new Regex("^[\\da-zA-Z]+$", RegexOptions.IgnoreCase);
            private static Regex _maxString = new Regex("^[א-ת\\\"\\-\\d\\s\\w\\/()]+$", RegexOptions.IgnoreCase);
            private static Regex phone = new Regex("^[\\d\\-+]+$");
            private static Regex time = new Regex("^[\\d\\:]{5}$");
            private static Regex email = new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");

            public static bool LettersEngNoSpace(string value, bool nullable)
            {
                if (nullable && string.IsNullOrWhiteSpace(value)) { return true; }
                return lettersEngNoSpace.IsMatch(value);
            }
            public static bool Phone(string value, bool nullable = true)
            {
                if (nullable && string.IsNullOrWhiteSpace(value)) { return true; }
                return phone.IsMatch(value);
            }
            public static bool Time(string value)
            {
                //,'02:00:00'
                //,'23:30:00'
                if (string.IsNullOrWhiteSpace(value)) { return true; }
                return time.IsMatch(value);
            }
            public static bool Email(string value)
            {
                if (string.IsNullOrWhiteSpace(value)) { return false; }
                return email.IsMatch(value);
            }
            public static bool DigitsOnly(string value, bool nullable)
            {
                if (nullable && string.IsNullOrWhiteSpace(value)) { return true; }
                return digitsOnly.IsMatch(value);
            }
            public static bool DigitsDash(string value, bool nullable)
            {
                if (nullable && string.IsNullOrWhiteSpace(value)) { return true; }
                return digitsDash.IsMatch(value);
            }
            public static bool LettersEngDigits(string value, bool nullable)
            {
                if (nullable && string.IsNullOrWhiteSpace(value)) { return true; }
                return lettersEngDigits.IsMatch(value);
            }
            public static bool LettersOnly(string value, bool nullable)
            {
                if (nullable && string.IsNullOrWhiteSpace(value)) { return true; }
                return lettersOnly.IsMatch(value);
            }
            public static bool LettersQuoteDash(string value, bool nullable)
            {
                if (nullable && string.IsNullOrWhiteSpace(value)) { return true; }
                return lettersQuoteDash.IsMatch(value);
            }
            public static bool LettersQuoteDigits(string value, bool nullable)
            {
                if (nullable && string.IsNullOrWhiteSpace(value)) { return true; }
                return lettersQuoteDigits.IsMatch(value);
            }
            public static bool maxString(string value)
            {
                if (string.IsNullOrWhiteSpace(value)) { return true; }
                return _maxString.IsMatch(value);
            }
            public static bool Password(string value)
            {
                if (string.IsNullOrWhiteSpace(value)) { return false; }
                //          10 chars            has upper case and lower          and number        and symbol
                //if (p && p.length >= 10 && /[A-Z] /.test(p) && /[a-z] /.test(p) && /\d/.test(p) && /\W/.test(p)){
                string p = value;
                if (p.Length >= 10 && Regex.IsMatch(p, "[a-z]") && Regex.IsMatch(p, "[A-Z]")
                    && Regex.IsMatch(p, "[\\d]") && Regex.IsMatch(p, "[\\W]"))
                {
                    return true;
                }
                return false;
            }
        }


        //private static readonly CacheItemPolicy policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(5) };
        //private static readonly ObjectCache cache = MemoryCache.Default;
        private static readonly string ConStr = ConfigurationManager.AppSettings["SqlConnectionString"];
        public static Exception LastException { get; private set; }



        /***************/
        /***** exec ****/
        /***************/

        public static int exec(string cmdString)
        {
            using (SqlCommand _cmd = new SqlCommand(cmdString))
            {
                return exec(_cmd);
            }
        }

        private static int exec(SqlCommand cmd)
        {
            try
            {
                //Logger.log("DAL.exec(" + cmd.CommandText + ")");
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    cmd.Connection = con;
                    int result = cmd.ExecuteNonQuery();
                    con.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                LastException = ex;
                // Logger.error("DAL.exec(" + cmd.CommandText + ")", ex);
                return -1;
            }
        }

        public static int exec(string cmdString, SqlParameter[] sqlParams)
        {
            using (SqlCommand _cmd = new SqlCommand(cmdString))
            {
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    _cmd.Parameters.Add(sqlParams[i]);
                }
                return exec(_cmd);
            }
        }





        /*****************/
        /***** select ****/
        /*****************/

        private static T[] MapFromDataTable<T>(DataTable t)
        {
            if (t == null)
            {
                return null;
            }

            List<T> items = new List<T>();
            for (int i = 0; i < t.Rows.Count; i++)
            {
                items.Add(Activator.CreateInstance<T>());
            }

            PropertyInfo[] pis = typeof(T).GetProperties();

            List<string> piNames = pis.Select(pi => pi.Name).ToList();
            IEnumerable<string> cNames = t.Columns.Cast<DataColumn>().Select(c => c.ColumnName);

            for (int i = 0; i < piNames.Count; i++)
            {
                string name = piNames[i];
                if (cNames.Contains(name))
                {
                    for (int j = 0; j < items.Count; j++)
                    {
                        if (t.Rows[j][name] != DBNull.Value)
                        {
                            //t.Rows[3][8].GetType().Name  |   t.Columns[8].DataType.Name
                            if (t.Columns[name].DataType.Name == "TimeSpan")
                            {
                                pis[i].SetValue(items[j], t.Rows[j][name].ToString().Substring(0, 5));
                            }
                            else if (t.Columns[name].DataType.Name == "Decimal")
                            {
                                // pis[i].SetValue(items[j], Convert.ToDouble(t.Rows[j][name]));
                            }
                            else
                            {
                                pis[i].SetValue(items[j], t.Rows[j][name]);
                            }
                        }
                    }
                }
            }

            return items.ToArray();
        }

        public static T[] select<T>(string cmdString)
        {
            using (SqlCommand _cmd = new SqlCommand(cmdString))
            {
                DataTable t = select(_cmd);
                return MapFromDataTable<T>(t);
            }
        }

        public static T[] select<T>(string cmdString, SqlParameter[] sqlParams)
        {
            using (SqlCommand _cmd = new SqlCommand(cmdString))
            {
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    _cmd.Parameters.Add(sqlParams[i]);
                }
                DataTable t = select(_cmd);
                return MapFromDataTable<T>(t);
            }
        }

        public static DataTable select(string cmdString)
        {
            using (SqlCommand _cmd = new SqlCommand(cmdString))
            {
                return select(_cmd);
            }
        }

        public static DataTable select(string cmdString, SqlParameter[] sqlParams)
        {
            using (SqlCommand _cmd = new SqlCommand(cmdString))
            {
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    _cmd.Parameters.Add(sqlParams[i]);
                }
                return select(_cmd);
            }
        }

        private static DataTable select(SqlCommand cmd)
        {
            try
            {
                //Logger.log("DAL.select(" + cmd.CommandText + ")");

                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter())
                    {
                        DataSet ds = new DataSet("ds");
                        cmd.Connection = con;
                        adp.SelectCommand = cmd;
                        adp.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LastException = ex;
                // Logger.error("DAL.select(" + cmd.CommandText + ")", ex);
                return null;
            }
        }


        /*****************/
        /***** insert ****/
        /*****************/

        public static int insert<T>(T data)
        {
            string tableName = data.GetType().Name;
            return insert(tableName, data);
        }

        public static int insert<T>(string tableName, T data)
        {
            using (var connection = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PropertyInfo[] props = data.GetType().GetProperties();
                    string cols = string.Empty;
                    string vals = string.Empty;

                    //insertCommand.Parameters.AddWithValue("@TransactionNumber", transaction.TransactionNumber);
                    foreach (PropertyInfo pi in props)
                    {
                        if (pi.Name.ToLower() == "id") { continue; }
                        if (pi.Name.ToLower() == tableName.ToLower() + "id") { continue; }
                        if (pi.PropertyType.IsClass == true && pi.PropertyType != typeof(string)) { continue; }
                        if (pi.PropertyType.IsArray == true) { continue; }
                        if (pi.PropertyType == typeof(string) && Validators.maxString(pi.GetValue(data).ToString()) == false)
                        {
                            //ooohhhh injection
                            throw new OperationCanceledException("injections ohh yee:: " + pi.GetValue(data).ToString());
                        }
                        if (pi.GetValue(data) == null)
                        {
                            continue;
                        }
                        cmd.Parameters.AddWithValue("@" + pi.Name, pi.GetValue(data));
                        cols += pi.Name + ',';
                        vals += '@' + pi.Name + ',';
                    }

                    cols = cols.Substring(0, cols.Length - 1);
                    vals = vals.Substring(0, vals.Length - 1);
                    cmd.Connection = connection;
                    cmd.CommandText = "insert into " + tableName + " (" + cols + ") values (" + vals + ");";

                    try
                    {
                        connection.Open();
                        //return cmd.ExecuteNonQuery();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected != 1)
                        {
                            return -1;
                        }

                        using (SqlCommand cmd2 = new SqlCommand("select @@IDENTITY", connection))
                        {
                            using (SqlDataAdapter adp = new SqlDataAdapter())
                            {
                                DataSet ds = new DataSet("ds");
                                adp.SelectCommand = cmd2;
                                adp.Fill(ds);
                                if (ds.Tables[0].Rows[0][0] == DBNull.Value)
                                {
                                    return -2;
                                }
                                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        // raised if insert fails (or other reasons e. g. missing permission)
                        Console.WriteLine(ex.Message);
                        return -1;
                    }
                }
            }
        }


        /*****************/
        /***** update ****/
        /*****************/

        public static int update(string cmdString, SqlParameter[] sqlParams = null)
        {
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {
                    try
                    {
                        if (sqlParams != null)
                        {
                            cmd.Parameters.AddRange(sqlParams);
                        }
                        connection.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // raised if insert fails (or other reasons e. g. missing permission)
                        Console.WriteLine(ex.Message);
                        return -1;
                    }
                }
            }
        }

        public static int update<T>(string tableName, T data, int id)
        {
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PropertyInfo[] props = data.GetType().GetProperties();
                    string sets = string.Empty;

                    //insertCommand.Parameters.AddWithValue("@TransactionNumber", transaction.TransactionNumber);
                    bool first = true;
                    foreach (PropertyInfo pi in props)
                    {
                        if (pi.Name.ToLower() == "id") { continue; }
                        if (pi.Name.ToLower() == tableName.ToLower() + "id") { continue; }
                        string value = pi.Name.ToLower();
                        string value2 = tableName.ToLower() + "id";
                        if (pi.PropertyType.IsClass == true && pi.PropertyType != typeof(string)) { continue; }
                        if (pi.PropertyType == typeof(string) && Validators.maxString(pi.GetValue(data).ToString()) == false)
                        {
                            //ooohhhh injection
                            throw new OperationCanceledException("injections ohh yee:: " + pi.GetValue(data).ToString());
                        }

                        cmd.Parameters.AddWithValue("@" + pi.Name, pi.GetValue(data));
                        if (first == true)
                        {
                            first = false;
                        }
                        else
                        {
                            sets += ", ";
                        }
                        sets += pi.Name + " = @" + pi.Name + " ";
                    }

                    cmd.Connection = connection;

                    cmd.CommandText = "UPDATE " + tableName + " SET " + sets + " WHERE " + tableName + "ID = " + id;

                    try
                    {
                        connection.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // raised if insert fails (or other reasons e. g. missing permission)
                        Console.WriteLine(ex.Message);
                        return -1;
                    }
                }
            }
        }


        public static int Delete(string tableName, int id)
        {
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM " + tableName + " WHERE " + tableName + "ID = " + id;

                    try
                    {
                        connection.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // raised if insert fails (or other reasons e. g. missing permission)
                        Console.WriteLine(ex.Message);
                        return -1;
                    }
                }
            }
        }
    

    }
}