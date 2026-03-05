using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public class DBConnection
    {
        private const string ConnectionString = "User Id=system;Password=oracle;Data Source=127.0.0.1:1522/XEPDB1;";

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
            using (OracleConnection connection = GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
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
    }
}