using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KHSC.DAO.Others;

namespace KHSC.Gateway.Others
{
    public class TerriotoryGateway
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());

        internal string GetTerriotoryAutoId()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT 'Terriotory-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([empter_id],6))),0)+1),6) FROM [tbl_employee_territory_information]";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                return command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        internal DataTable GetAllTerriotoryInformaation()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [empter_id]
      ,[empter_terriotory_name]
  FROM [tbl_employee_territory_information]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_department_information");
                DataTable table = ds.Tables["tbl_department_information"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }
            }
        }

        internal void SaveTheTerrioToryInformation(Terriotory aTerriotoryObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"INSERT INTO [tbl_employee_territory_information]
           ([empter_id]
           ,[empter_terriotory_name])
     VALUES
           ('" + aTerriotoryObj.TerriotoryId + "','" + aTerriotoryObj.TerrioToryName + "')";
                SqlCommand command = new SqlCommand(selectQuery,connection);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }
            }
        }

        internal void UpdateTheTerrioToryInformation(Terriotory aTerriotoryObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"UPDATE [tbl_employee_territory_information]
   SET [empter_terriotory_name] ='" + aTerriotoryObj.TerrioToryName + "'  WHERE [empter_id] ='" + aTerriotoryObj.TerriotoryId + "'  ";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }
            }
        }

        internal void DeleteTheTerrioToryInformation(Terriotory aTerriotoryObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"DELETE FROM [tbl_employee_territory_information] WHERE [empter_id] ='" + aTerriotoryObj.TerriotoryId + "'  ";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }
            }
        }
    }
}