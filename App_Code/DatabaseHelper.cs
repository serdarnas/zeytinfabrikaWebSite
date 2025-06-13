using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public static class DatabaseHelper
{
    private static string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString; }
    }

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }

    public static int ExecuteNonQuery(string commandText, List<SqlParameter> parameters = null, SqlTransaction transaction = null)
    {
        try
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    if (transaction != null)
                        cmd.Transaction = transaction;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    return cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError(ex);
            throw;
        }
    }

    public static object ExecuteScalar(string commandText, List<SqlParameter> parameters = null, SqlTransaction transaction = null)
    {
        try
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    if (transaction != null)
                        cmd.Transaction = transaction;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    return cmd.ExecuteScalar();
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError(ex);
            throw;
        }
    }

    public static SqlDataReader ExecuteReader(string commandText, List<SqlParameter> parameters = null, SqlConnection connection = null, SqlTransaction transaction = null)
    {
        try
        {
            SqlConnection conn = connection ?? GetConnection();
            if (connection == null)
                conn.Open();

            SqlCommand cmd = new SqlCommand(commandText, conn);
            if (transaction != null)
                cmd.Transaction = transaction;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());

            return cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError(ex);
            throw;
        }
    }

    public static DataTable GetDataTable(string commandText, List<SqlParameter> parameters = null)
    {
        try
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError(ex);
            throw;
        }
    }

    public static void BeginTransaction(SqlConnection connection, out SqlTransaction transaction)
    {
        if (connection.State != ConnectionState.Open)
            connection.Open();

        transaction = connection.BeginTransaction();
    }

    public static void CommitTransaction(SqlTransaction transaction)
    {
        if (transaction != null)
            transaction.Commit();
    }

    public static void RollbackTransaction(SqlTransaction transaction)
    {
        if (transaction != null)
            transaction.Rollback();
    }
}
