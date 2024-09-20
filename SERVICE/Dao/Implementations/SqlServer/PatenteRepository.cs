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
    /// Repositorio para la gestión de entidades Patente con operaciones CRUD.
    /// </summary>
    public sealed class PatenteRepository : IGenericDao<Patente>
    {
        #region Singleton Pattern
        private static readonly PatenteRepository _instance = new PatenteRepository();

        /// <summary>
        /// Acceso a la instancia singleton del repositorio.
        /// </summary>
        public static PatenteRepository Current => _instance;

        private PatenteRepository()
        {
            // Inicialización opcional del singleton.
        }
        #endregion

        /// <summary>
        /// Añade una nueva Patente al sistema.
        /// </summary>
        /// <param name="obj">La instancia de Patente a añadir.</param>
        public void Add(Patente obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "La instancia de Patente no puede ser nula.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("PatenteInsert", CommandType.StoredProcedure,
                    new SqlParameter("@IdPatente", obj.Id),
                    new SqlParameter("@Nombre", obj.Nombre),
                    new SqlParameter("@DataKey", obj.DataKey),
                    new SqlParameter("@TipoAcceso", (int)obj.TipoAcceso));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al añadir la Patente.", ex);
            }
        }

        /// <summary>
        /// Actualiza una Patente existente en el sistema.
        /// </summary>
        /// <param name="obj">La instancia de Patente a actualizar.</param>
        public void Update(Patente obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "La instancia de Patente no puede ser nula.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("PatenteUpdate", CommandType.StoredProcedure,
                    new SqlParameter("@IdPatente", obj.Id),
                    new SqlParameter("@Nombre", obj.Nombre),
                    new SqlParameter("@DataKey", obj.DataKey),
                    new SqlParameter("@TipoAcceso", (int)obj.TipoAcceso));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar la Patente.", ex);
            }
        }

        /// <summary>
        /// Elimina una Patente del sistema por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la Patente a eliminar.</param>
        public void Remove(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("El identificador de la Patente no puede estar vacío.", nameof(id));
            }

            try
            {
                SqlHelper.ExecuteNonQuery("PatenteDelete", CommandType.StoredProcedure,
                    new SqlParameter("@IdPatente", id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar la Patente.", ex);
            }
        }

        /// <summary>
        /// Obtiene una Patente por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la Patente.</param>
        /// <returns>La instancia de Patente si existe, de lo contrario, null.</returns>
        public Patente GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("El identificador de la Patente no puede estar vacío.", nameof(id));
            }

            Patente patente = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader("PatenteSelect", CommandType.StoredProcedure,
                    new SqlParameter("@IdPatente", id)))
                {
                    if (reader.Read())
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        patente = PatenteMapper.Current.Fill(data);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener la Patente por Id.", ex);
            }

            return patente;
        }

        /// <summary>
        /// Obtiene todas las Patentes del sistema.
        /// </summary>
        /// <returns>Una lista de todas las instancias de Patente.</returns>
        public List<Patente> GetAll()
        {
            List<Patente> patentes = new List<Patente>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader("PatenteSelectAll", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        patentes.Add(PatenteMapper.Current.Fill(data));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener todas las Patentes.", ex);
            }

            return patentes;
        }
    }
}
