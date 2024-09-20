using SERVICE.Dao.Contracts;
using Services.Dao.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Dao.Helpers
{
    public class RoleRepository : IRoleRepository
    {
        // Método para buscar un rol por su ID
        public Patente FindById(int id)
        {
            Patente role = null;

            using (var reader = SqlHelper.ExecuteReader("RolSelectById", CommandType.StoredProcedure,
                new SqlParameter("@IdRol", id)))
            {
                if (reader.Read())
                {
                    role = new Patente
                    {
                        Id = reader.GetGuid(0),
                        Nombre = reader.GetString(1),
                        // Asignar más propiedades de Patente si las tienes
                    };
                }
            }

            return role;
        }

        // Implementación explícita de la interfaz para devolver un Acceso
        Acceso IRoleRepository.FindById(int id)
        {
            return FindById(id); // Devuelve el mismo objeto, ya que Patente es un Acceso
        }
    }
}
