using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;


namespace KHSC
{
    /// <summary>
    /// Summary description for IdManager.
    /// </summary>
    public class IdManager
    {
        private static decimal TotalUnitPrice = 0.0M;
        
        public static int GetNextID(string tableName, string idField)
        {
            int val=0;
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);

            string Query = "select isnull(max(convert(int,(" + idField + "))),0) from  " + tableName;
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            if (maxValue == DBNull.Value) return 1;
            else
                val = int.Parse((maxValue).ToString());
            val= val + 1;
            return val;
        }

        public static int GetNextSl(string tableName, string idField,string criField, Int32 criteria)
        {
            int val;
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);

            string Query = "select max(" + idField + ") from gl_trans_dtl where vch_sys_no = "+criteria;
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            if (maxValue == DBNull.Value) return 1;
            else
                val = int.Parse((maxValue).ToString());
            return val + 1;
        }

        public static int GetNextEmpSl(string criteria)
        {
            int val;
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);

            string Query = "select count(*) from pmis_personnel where substring(emp_no,1,8) = '" + criteria + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            if (maxValue == DBNull.Value) return 1;
            else
                val = int.Parse((maxValue).ToString());
            return val + 1;
        }

        public static string GetItemSl(string criteria)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);

            string Query = "select ltrim(convert(varchar,nullif(max(convert(substr(item_code,-3))),0)+1,'000')) from item_mst where item_code like '" + criteria + "%'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();

            return maxValue.ToString();
        }

        public static string GetTransSl(string tableName, string idField)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);

            string Query = "select rtrim(ltrim(convert(varchar,sysdate,'RR')))||ltrim(convert(varchar,nullif(max(convert(substr(rtrim(ltrim(" + idField + ")),4))),0)+1,'000000')) from " + tableName;
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();

            return maxValue.ToString();
        }

        public static int GetNextSlStd()
        {
            //int val;
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);

            //string Query = "select isnull(max(convert(numeric,substring(student_id,5,5))),0) sid from student_info ";
            //myConnection.Open();
            //SqlCommand myCommand = new SqlCommand(Query, myConnection);
            //object maxValue = myCommand.ExecuteScalar();
            //myConnection.Close();
            //if (maxValue == DBNull.Value) return 1;
            //else
            //    val = int.Parse((maxValue).ToString());
            //return val + 1;

            try
            {
                myConnection.Open();
                string Query = "select isnull(max(student_id),0) sid from student_info ";
                SqlCommand command = new SqlCommand(Query,myConnection);
                int maxValue = Convert.ToInt32(command.ExecuteScalar());
                if (maxValue >= 0)
                {
                    return maxValue + 1;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        
        public static decimal GetUnitPrice(decimal Price)
        {
            if (Price != 0)
            {
                TotalUnitPrice += Price;
            }
            else
            {
                Price = 0;
            }
            return Price;
            
        }

        public static decimal GetTotal()
        {
            return TotalUnitPrice;
        }
        public static string getColName(string comm)
        {
            string colname = "";
            string connectionString = DataManager.OraConnString();
            SqlDataReader dReader;
            SqlCommand cmd;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select lower(table_name)||'.'||lower(column_name) col_name from user_col_comments where comments= '" + comm + "' ";
            dReader = cmd.ExecuteReader();
            if (dReader.HasRows == true)
            {
                while (dReader.Read())
                {
                    colname = dReader["col_name"].ToString();
                }
            }
            return colname;
        }

        public static int GetShowSingleValueInt(string ShowField, string SearchField, string TableName, string Parameter)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            try
            {
                connection.Open();
                string Query = "select " + ShowField + " from " + TableName + " where " + SearchField + "='" + Parameter + "' ";
                SqlCommand command = new SqlCommand(Query, connection);
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
        public static int GetShowSingleValueIntWithNoParameter(string ShowField, string TableName)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            try
            {
                connection.Open();
                string Query = "select " + ShowField + " from " + TableName + "";
                SqlCommand command = new SqlCommand(Query, connection);
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


        public static string GetShowSingleValueString(string ShowField, string SearchField, string TableName, string Parameter)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            try
            {
                connection.Open();
                string Query = "select " + ShowField + " from " + TableName + " where " + SearchField + "='" + Parameter + "' ";
                SqlCommand command = new SqlCommand(Query, connection);
                string Value = (string)command.ExecuteScalar();
                return Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public static double GetShowSingleValueCurrency(string ShowField, string SearchField, string TableName, string Parameter)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            try
            {
                connection.Open();
                string Query = "select isnull(" + ShowField + ",0) from " + TableName + " where " + SearchField + "='" + Parameter + "' ";
                SqlCommand command = new SqlCommand(Query, connection);
                return Convert.ToDouble(command.ExecuteScalar());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }



        public static DataTable GetShowDataTable(string query)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection oracon = new SqlConnection(connectionString);
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Table");
            return dt;
        }
    }

}
