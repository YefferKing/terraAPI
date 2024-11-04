using MySql.Data.MySqlClient;
using System;

namespace TerraApi.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public T ExecuteQuery<T>(string query, Func<MySqlDataReader, T> processResult)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        return processResult(reader);
                    }
                }
            }
        }
    }
}
