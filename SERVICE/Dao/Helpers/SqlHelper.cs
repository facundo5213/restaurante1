using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Services.Dao.Helpers
{
    /// <summary>
    /// Proporciona métodos estáticos para ejecutar operaciones en la base de datos.
    /// </summary>
    public static class SqlHelper
    {
        // Obtener la cadena de conexión desde App.config
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SeguridadRestauranteConnection"].ConnectionString;
        }

        // Método para ejecutar comandos que no devuelven resultados (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string sqlQuery, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Método para ejecutar consultas que devuelven un SqlDataReader (SELECT)
        public static SqlDataReader ExecuteReader(string sqlQuery, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);
            conn.Open();

            // Retornar el lector de datos y dejar la conexión abierta
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}