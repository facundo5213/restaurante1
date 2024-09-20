using Services.Dao.Contracts;
using Services.Dao.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Services.Dao.Implementations.SqlServer
{
    /// <summary>
    /// Repositorio para manejar las relaciones entre Usuario y Patente.
    /// </summary>
    public sealed class UsuarioPatenteRepository
    {
        #region Singleton Pattern
        private static readonly UsuarioPatenteRepository _instance = new UsuarioPatenteRepository();

        /// <summary>
        /// Acceso a la instancia singleton del repositorio.
        /// </summary>
        public static UsuarioPatenteRepository Current => _instance;

        private UsuarioPatenteRepository()
        {
            // Inicialización opcional del singleton.
        }
        #endregion

        /// <summary>
        /// Añade una relación entre Usuario y Patente.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        /// <param name="patente">La instancia de Patente.</param>
        public void Add(Usuario usuario, Patente patente)
        {
            if (usuario == null || patente == null)
            {
                throw new ArgumentNullException("Usuario y Patente no pueden ser nulos.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("Usuario_PatenteInsert", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", usuario.IdUsuario),
                    new SqlParameter("@IdPatente", patente.Id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al añadir la relación entre Usuario y Patente.", ex);
            }
        }

        /// <summary>
        /// Elimina una relación entre Usuario y Patente.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        /// <param name="patente">La instancia de Patente.</param>
        public void Remove(Usuario usuario, Patente patente)
        {
            if (usuario == null || patente == null)
            {
                throw new ArgumentNullException("Usuario y Patente no pueden ser nulos.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("Usuario_PatenteDelete", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", usuario.IdUsuario),
                    new SqlParameter("@IdPatente", patente.Id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar la relación entre Usuario y Patente.", ex);
            }
        }

        /// <summary>
        /// Elimina todas las relaciones de un Usuario.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        public void RemoveByUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "Usuario no puede ser nulo.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("Usuario_PatenteDeleteByIdUsuario", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", usuario.IdUsuario));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar las relaciones del Usuario.", ex);
            }
        }

        /// <summary>
        /// Elimina todas las relaciones de una Patente.
        /// </summary>
        /// <param name="patente">La instancia de Patente.</param>
        public void RemoveByPatente(Patente patente)
        {
            if (patente == null)
            {
                throw new ArgumentNullException(nameof(patente), "Patente no puede ser nula.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("Usuario_PatenteDeleteByIdPatente", CommandType.StoredProcedure,
                    new SqlParameter("@IdPatente", patente.Id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar las relaciones de la Patente.", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las Patentes asociadas a un Usuario.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        /// <returns>Una lista de Patentes asociadas al Usuario.</returns>
        public List<Patente> GetPatentesByUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "Usuario no puede ser nulo.");
            }

            List<Patente> patentes = new List<Patente>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader("Usuario_PatenteSelectByIdUsuario", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", usuario.IdUsuario)))
                {
                    while (reader.Read())
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);

                        // En lugar de hacer múltiples llamadas a GetById, carga la patente aquí.
                        Guid patenteId = Guid.Parse(data[1].ToString());
                        string nombrePatente = data[2].ToString(); // Supongamos que el nombre está en la columna 2.
                        string dataKey = data[3].ToString();
                        int tipoAcceso = int.Parse(data[4].ToString());

                        patentes.Add(new Patente
                        {
                            Id = patenteId,
                            Nombre = nombrePatente,
                            DataKey = dataKey,
                            TipoAcceso = (TipoAcceso)tipoAcceso
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener las Patentes del Usuario.", ex);
            }

            return patentes;
        }

        /// <summary>
        /// Asocia las Patentes correspondientes a un Usuario.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        public void Join(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "Usuario no puede ser nulo.");
            }

            usuario.Accesos.AddRange(GetPatentesByUsuario(usuario));
        }
    }
}
