using MySql.Data.MySqlClient;
using System;
using System.Data;
using TerraApi.Modelos.Usuario;

using System.Security.Cryptography;
using System.Text;
using TerraApi.Helpers;

namespace TerraApi.Dao.Usuario
{
    public class UsuarioDao
    {
        private readonly string _connectionString;
        private readonly DatabaseHelper _databaseHelper;

        public UsuarioDao(string connectionString, DatabaseHelper databaseHelper)
        {
            _connectionString = connectionString;
            _databaseHelper = databaseHelper;
        }


        public class LoginResponse
        {
            public UsuarioData UsuarioData { get; set; }
            public string Message { get; set; }
        }

        public LoginResponse Login(string username, string password)
        {
            string query = $"CALL LOGIN('{username}', '{password}')";

            // Ejecuta la consulta y procesa el resultado
            return _databaseHelper.ExecuteQuery(query, reader =>
            {
                string message = null;
                UsuarioData usuarioData = null;

                if (reader.Read())
                {
                    // Comprueba si hay un mensaje de error
                    if (!reader.IsDBNull(reader.GetOrdinal("Message")))
                    {
                        message = reader.GetString("Message");
                    }

                    // Si hay un mensaje de éxito, obtenemos los datos del usuario
                    if (message == "Autenticación Exitosa" && !reader.IsDBNull(reader.GetOrdinal("USUARIO")))
                    {
                        usuarioData = new UsuarioData
                        {
                            Usuario = reader.GetString("USUARIO"),
                            Contraseña = null
                        };
                    }
                }
                else
                {
                    message = "Usuario no encontrado";
                }

                // Devuelve un nuevo LoginResponse con los resultados
                return new LoginResponse
                {
                    Message = message,
                    UsuarioData = usuarioData
                };
            });
        }

    }
}
