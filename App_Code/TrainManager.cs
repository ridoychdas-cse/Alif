﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for TrainManager
/// </summary>
/// 
namespace KHSC
{
    public class TrainManager
    {
        public static void CreateTrain(Train trn)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into pmis_train_dtl (emp_no,train_title,year,place,country,finan,amount,du_year,du_month,du_day) values (" +
                " " + " '" + trn.EmpNo + "'," + " '" + trn.TrainTitle + "'," + " " + trn.Year + "," + " '" + trn.Place + "', " +
             " " + " '" + trn.Country + "'," + " '" + trn.Finan + "'," + " " + trn.Amount + "," + " " + trn.DuYear + "," + " " + trn.DuMonth + "," + " " + trn.DuDay + ")";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateTrain(Train trn)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update pmis_train_dtl set train_title = '" + trn.TrainTitle + "',year= " + trn.Year + ",place= '" + trn.Place + "', " +
             " country= '" + trn.Country + "', finan = '" + trn.Finan + "', amount = " + trn.Amount + ", du_year="+trn.DuYear+" "+
             " du_month="+trn.DuMonth+",du_day="+trn.DuDay+" where emp_no='" + trn.EmpNo + "' and train_title='" + trn.TrainTitle + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteTrain(string emp)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from pmis_train_dtl where emp_no='" + emp + "' ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static Train getTrain(string empno, string trn)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, TRAIN_TITLE, PLACE, COUNTRY, FINAN, convert(AMOUNT)amount, convert(YEAR) year, convert(DU_YEAR) du_year, " +
                " convert(DU_MONTH)du_month, convert(DU_DAY)du_day from pmis_train_dtl where emp_no='" + empno + "' and rtrim(train_title)= rtrim('" + trn + "') ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Training");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Train(dt.Rows[0]);
        }
        public static DataTable getTrains(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, TRAIN_TITLE, PLACE, COUNTRY, FINAN, convert(AMOUNT)amount, convert(YEAR) year, convert(DU_YEAR) du_year, "+
                " convert(DU_MONTH)du_month, convert(DU_DAY)du_day from pmis_train_dtl where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Training");
            return dt;
        }
        public static DataTable getTraining(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, dbo.initcap(TRAIN_TITLE)train_title, dbo.initcap(PLACE)place, COUNTRY, FINAN, convert(AMOUNT)amount, convert(YEAR) year, convert(DU_YEAR) du_year, " +
                " convert(DU_MONTH)du_month, convert(DU_DAY)du_day from pmis_train_dtl where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Training");
            return dt;
        }
        public static DataTable getTrainingRpt(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, dbo.initcap(TRAIN_TITLE)train_title, dbo.initcap(PLACE)place, COUNTRY, FINAN, convert(AMOUNT)amount, convert(YEAR) year, convert(DU_YEAR) du_year, " +
                " convert(DU_MONTH)du_month, convert(DU_DAY)du_day from pmis_train_dtl ";
            if (criteria.Length > 0)
            {
                query += " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Training");
            return dt;
        }
        public static DataTable getTrainRpt(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select dbo.initcap(TRAIN_TITLE)train_title, convert(YEAR) year,dbo.initcap(PLACE)place, COUNTRY, FINAN, convert(AMOUNT)amount,  convert(DU_YEAR) du_year, " +
                " convert(DU_MONTH)du_month, convert(DU_DAY)du_day from pmis_train_dtl where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Training");
            return dt;
        }
    }
}
