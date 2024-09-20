using Services.Dao.Contracts;
using Services.Dao.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Services.Dao.Implementations.SqlServer
{
    /// <summary>
    /// Repositorio para manejar las relaciones entre Usuario y Familia.
    /// </summary>
    public sealed class UsuarioFamiliaRepository
    {
        #region Singleton Pattern
        private static readonly UsuarioFamiliaRepository _instance = new UsuarioFamiliaRepository();

        /// <summary>
        /// Acceso a la instancia singleton del repositorio.
        /// </summary>
        public static UsuarioFamiliaRepository Current => _instance;

        private UsuarioFamiliaRepository()
        {
            // Inicialización opcional del singleton.
        }
        #endregion

        /// <summary>
        /// Añade una relación entre Usuario y Familia.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        /// <param name="familia">La instancia de Familia.</param>
        public void Add(Usuario usuario, Familia familia)
        {
            if (usuario == null || familia == null)
            {
                throw new ArgumentNullException("Usuario y Familia no pueden ser nulos.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("Usuario_FamiliaInsert", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", usuario.IdUsuario),
                    new SqlParameter("@IdFamilia", familia.Id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al añadir la relación entre Usuario y Familia.", ex);
            }
        }

        /// <summary>
        /// Elimina una relación entre Usuario y Familia.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        /// <param name="familia">La instancia de Familia.</param>
        public void Remove(Usuario usuario, Familia familia)
        {
            if (usuario == null || familia == null)
            {
                throw new ArgumentNullException("Usuario y Familia no pueden ser nulos.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("Usuario_FamiliaDelete", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", usuario.IdUsuario),
                    new SqlParameter("@IdFamilia", familia.Id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar la relación entre Usuario y Familia.", ex);
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
                SqlHelper.ExecuteNonQuery("Usuario_FamiliaDeleteByIdUsuario", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", usuario.IdUsuario));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar las relaciones del Usuario.", ex);
            }
        }

        /// <summary>
        /// Elimina todas las relaciones de una Familia.
        /// </summary>
        /// <param name="familia">La instancia de Familia.</param>
        public void RemoveByFamilia(Familia familia)
        {
            if (familia == null)
            {
                throw new ArgumentNullException(nameof(familia), "Familia no puede ser nula.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("Usuario_FamiliaDeleteByIdFamilia", CommandType.StoredProcedure,
                    new SqlParameter("@IdFamilia", familia.Id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar las relaciones de la Familia.", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las Familias asociadas a un Usuario.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        /// <returns>Una lista de Familias asociadas al Usuario.</returns>
        public List<Familia> GetFamiliasByUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "Usuario no puede ser nulo.");
            }

            List<Familia> familias = new List<Familia>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader("Usuario_FamiliaSelectByIdUsuario", CommandType.StoredProcedure,
                    new SqlParameter("@IdUsuario", usuario.IdUsuario)))
                {
                    while (reader.Read())
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);

                        // En lugar de hacer múltiples llamadas a GetById, carga la familia aquí.
                        Guid familiaId = Guid.Parse(data[1].ToString());
                        string nombreFamilia = data[2].ToString(); // Supongamos que el nombre está en la columna 2.
                        familias.Add(new Familia { Id = familiaId, Nombre = nombreFamilia });
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener las Familias del Usuario.", ex);
            }

            return familias;
        }

        /// <summary>
        /// Asocia las Familias correspondientes a un Usuario.
        /// </summary>
        /// <param name="usuario">La instancia de Usuario.</param>
        public void Join(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "Usuario no puede ser nulo.");
            }

            usuario.Accesos.AddRange(GetFamiliasByUsuario(usuario));
        }
    }
}
