using Dao.Contracts;
using Services.Dao.Helpers;
using Services.Dao.Implementations.SqlServer.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Services.Dao.Implementations.SqlServer
{
    /// <summary>
    /// Repositorio para la gestión de entidades Usuario con operaciones CRUD.
    /// </summary>
    public sealed class UsuarioRepository : IGenericDao<Usuario>
    {
        #region Singleton Pattern
        private static readonly UsuarioRepository _instance = new UsuarioRepository();

        /// <summary>
        /// Acceso a la instancia singleton del repositorio.
        /// </summary>
        public static UsuarioRepository Current => _instance;

        private UsuarioRepository()
        {
            // Inicialización opcional del singleton.
        }
        #endregion

        /// <summary>
        /// Añade un nuevo Usuario al sistema.
        /// </summary>
        /// <param name="obj">La instancia de Usuario a añadir.</param>
        public void Add(Usuario obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "La instancia de Usuario no puede ser nula.");
            }

            try
            {
                // Inserta el Usuario
                SqlHelper.ExecuteNonQuery("UsuarioInsert", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", obj.IdUsuario),
                    new SqlParameter("@UserName", obj.UserName),
                    new SqlParameter("@Password", obj.Password));

                // Inserta las relaciones de accesos (Patente y Familia)
                foreach (var acceso in obj.Accesos)
                {
                    if (acceso is Patente patente)
                    {
                        SqlHelper.ExecuteNonQuery("Usuario_PatenteInsert", CommandType.StoredProcedure,
                            new SqlParameter("@IdUsuario", obj.IdUsuario),
                            new SqlParameter("@IdPatente", patente.Id));
                    }
                    else if (acceso is Familia familia)
                    {
                        SqlHelper.ExecuteNonQuery("Usuario_FamiliaInsert", CommandType.StoredProcedure,
                            new SqlParameter("@IdUsuario", obj.IdUsuario),
                            new SqlParameter("@IdFamilia", familia.Id));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al añadir el Usuario y sus accesos.", ex);
            }
        }

        /// <summary>
        /// Actualiza un Usuario existente en el sistema.
        /// </summary>
        /// <param name="obj">La instancia de Usuario a actualizar.</param>
        public void Update(Usuario obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "La instancia de Usuario no puede ser nula.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("UsuarioUpdate", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", obj.IdUsuario),
                    new SqlParameter("@UserName", obj.UserName),
                    new SqlParameter("@Password", obj.Password));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar el Usuario.", ex);
            }
        }

        /// <summary>
        /// Elimina un Usuario del sistema por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del Usuario a eliminar.</param>
        public void Remove(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("El identificador de Usuario no puede estar vacío.", nameof(id));
            }

            try
            {
                SqlHelper.ExecuteNonQuery("UsuarioDelete", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar el Usuario.", ex);
            }
        }

        /// <summary>
        /// Obtiene un Usuario por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del Usuario.</param>
        /// <returns>La instancia de Usuario si existe, de lo contrario, null.</returns>
        public Usuario GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("El identificador de Usuario no puede estar vacío.", nameof(id));
            }

            Usuario usuario = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader("UsuarioSelect", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", id)))
                {
                    if (reader.Read())
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        usuario = UsuarioMapper.Current.Fill(data);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener el Usuario por Id.", ex);
            }

            return usuario;
        }

        /// <summary>
        /// Obtiene todos los Usuarios del sistema.
        /// </summary>
        /// <returns>Una lista de todas las instancias de Usuario.</returns>
        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader("UsuarioSelectAll", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        usuarios.Add(UsuarioMapper.Current.Fill(data));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener todos los Usuarios.", ex);
            }

            return usuarios;
        }
    }
}
