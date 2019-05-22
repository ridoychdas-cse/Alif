using System;
using System.Data;
using System.Configuration;
//using System.Linq;
//using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for UsersManager
/// </summary>
/// 
namespace KHSC
{
    public class UsersManager
    {
        public static void CreateUser(Users usr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "insert into utl_userinfo (user_name,password,description,user_grp,status,emp_no) values ( " +
                " '" + usr.UserName + "', '" + usr.Password + "', '" + usr.Description + "', " +
                " '" + usr.UserGrp + "', '" + usr.Status + "', '" + usr.EmpNo + "' ) ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateUser(Users usr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "update utl_userinfo set password= '" + usr.Password + "', description='" + usr.Description + "', " +
                " user_grp= '" + usr.UserGrp + "', status= '" + usr.Status + "', emp_no= '" + usr.EmpNo + "' where upper(user_name)=upper('"+usr.UserName+"')  ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteUser(Users usr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "delete from utl_userinfo where upper(user_name)=upper('"+usr.UserName+"')  ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static Users getUser(string usr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = "select * from utl_userinfo where upper(user_name)=upper('" + usr + "')";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "UserInfo");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Users(dt.Rows[0]);
        }
        public static DataTable GetUsers()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select upper(user_name) user_name,description,case user_grp when '1' then'Operator' when '2' then 'Supervisor' when '3' then 'Evaluator' when '4' then 'Administrator' end user_grp,user_grp usergrp from  utl_userinfo where status='A' order by user_name";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Users");
            return dt;
        }
        public static string getUserName(string user)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select dbo.initcap(description)  from utl_userinfo where user_name='" + user + "' ";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            string a = "";
            if (maxValue != null)
            {
                a = maxValue.ToString();
            }
            return a;
        }
    }
}

