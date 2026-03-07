using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public class DBConnection
    {
        private static string ConnectionString
        {
            get
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["MovieTicketDb"];
                if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                {
                    throw new InvalidOperationException("Database connection string 'MovieTicketDb' is missing in Web.config.");
                }

                return settings.ConnectionString;
            }
        }

        public OracleConnection GetConnection()
        {
            OracleConnection connection = new OracleConnection(ConnectionString);

            try
            {
                connection.Open();
                return connection;
            }
            catch (OracleException ex)
            {
                connection.Dispose();
                throw new InvalidOperationException("Failed to connect to the Oracle database.", ex);
            }
            catch (Exception)
            {
                connection.Dispose();
                throw;
            }
        }

        public DataTable ExecuteQuery(string query, IDictionary<string, object> parameters = null)
        {
            try
            {
                using (OracleConnection connection = GetConnection())
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.BindByName = true;

                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> parameter in parameters)
                        {
                            command.Parameters.Add(parameter.Key, parameter.Value ?? DBNull.Value);
                        }
                    }

                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        DataTable resultTable = new DataTable();
                        adapter.Fill(resultTable);
                        return resultTable;
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new InvalidOperationException("A database error occurred while retrieving data.", ex);
            }
        }
    }
}