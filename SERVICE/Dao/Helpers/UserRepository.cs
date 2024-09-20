using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Services.Domain;
using SERVICE.Dao.Contracts;
using Services.Dao.Helpers;

namespace SERVICE.Dao.Implementations.SqlServer
{
    public class UserRepository : IUserRepository
    {
        // Método para buscar un usuario por su nombre de usuario
        public Usuario FindByUsername(string username)
        {
            Usuario user = null;

            // Consulta SQL para buscar un usuario por nombre de usuario
            string sqlQuery = "SELECT IdUsuario, Username, Password, timestamp FROM dbo.Usuario WHERE Username = @Username";

            using (var reader = SqlHelper.ExecuteReader(sqlQuery, CommandType.Text,
                new SqlParameter("@Username", username)))
            {
                if (reader.Read())
                {
                    user = new Usuario
                    {
                        IdUsuario = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        timestamp = reader.GetDateTime(3),
                    };
                }
            }

            return user;
        }

        // Método para agregar un nuevo usuario a la base de datos
        public void Add(Usuario user)
        {
            // Consulta SQL para agregar un usuario
            string sqlQuery = @"INSERT INTO dbo.Usuario (IdUsuario, Username, Password, timestamp)
                                VALUES (@IdUsuario, @Username, @Password, @timestamp)";

            SqlHelper.ExecuteNonQuery(sqlQuery, CommandType.Text,
                new SqlParameter("@IdUsuario", user.IdUsuario),
                new SqlParameter("@Username", user.UserName),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@timestamp", user.timestamp));
        }

        // Método para obtener todos los usuarios
        public IEnumerable<Usuario> GetAll()
        {
            var users = new List<Usuario>();

            // Consulta SQL para obtener todos los usuarios
            string sqlQuery = "SELECT IdUsuario, Username, Password, timestamp FROM dbo.Usuario";

            using (var reader = SqlHelper.ExecuteReader(sqlQuery, CommandType.Text))
            {
                while (reader.Read())
                {
                    users.Add(new Usuario
                    {
                        IdUsuario = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        timestamp = reader.GetDateTime(3),
                    });
                }
            }

            return users;
        }

        // Método para buscar un usuario por su ID
        public Usuario FindById(int id)
        {
            Usuario user = null;

            // Consulta SQL para buscar un usuario por su ID
            string sqlQuery = "SELECT IdUsuario, Username, Password, timestamp FROM dbo.Usuario WHERE IdUsuario = @IdUsuario";

            using (var reader = SqlHelper.ExecuteReader(sqlQuery, CommandType.Text,
                new SqlParameter("@IdUsuario", id)))
            {
                if (reader.Read())
                {
                    user = new Usuario
                    {
                        IdUsuario = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        timestamp = reader.GetDateTime(3),
                    };
                }
            }

            return user;
        }

        public Usuario FindById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
