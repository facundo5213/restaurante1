﻿using Dao.Contracts;
using Services.Dao.Helpers;
using Services.Dao.Implementations.SqlServer.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Services.Dao.Implementations.SqlServer
{
    /// <summary>
    /// Repositorio para la gestión de entidades Familia con operaciones CRUD.
    /// </summary>
    public sealed class FamiliaRepository : IGenericDao<Familia>
    {
        #region Singleton Pattern
        private static readonly FamiliaRepository _instance = new FamiliaRepository();

        /// <summary>
        /// Acceso a la instancia singleton del repositorio.
        /// </summary>
        public static FamiliaRepository Current => _instance;

        private FamiliaRepository()
        {
            // Inicialización opcional del singleton.
        }
        #endregion

        /// <summary>
        /// Añade una nueva Familia al sistema.
        /// </summary>
        /// <param name="obj">La instancia de Familia a añadir.</param>
        public void Add(Familia obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "La instancia de Familia no puede ser nula.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("FamiliaInsert", CommandType.StoredProcedure,
                    new SqlParameter("@IdFamilia", obj.Id),
                    new SqlParameter("@Nombre", obj.Nombre));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al añadir la Familia.", ex);
            }
        }

        /// <summary>
        /// Actualiza una Familia existente en el sistema.
        /// </summary>
        /// <param name="obj">La instancia de Familia a actualizar.</param>
        public void Update(Familia obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "La instancia de Familia no puede ser nula.");
            }

            try
            {
                SqlHelper.ExecuteNonQuery("FamiliaUpdate", CommandType.StoredProcedure,
                    new SqlParameter("@IdFamilia", obj.Id),
                    new SqlParameter("@Nombre", obj.Nombre));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar la Familia.", ex);
            }
        }

        /// <summary>
        /// Elimina una Familia del sistema por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la Familia a eliminar.</param>
        public void Remove(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("El identificador de la Familia no puede estar vacío.", nameof(id));
            }

            try
            {
                SqlHelper.ExecuteNonQuery("FamiliaDelete", CommandType.StoredProcedure,
                    new SqlParameter("@IdFamilia", id));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar la Familia.", ex);
            }
        }

        /// <summary>
        /// Obtiene una Familia por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la Familia.</param>
        /// <returns>La instancia de Familia si existe, de lo contrario, null.</returns>
        public Familia GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("El identificador de la Familia no puede estar vacío.", nameof(id));
            }

            Familia familia = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader("FamiliaSelect", CommandType.StoredProcedure,
                    new SqlParameter("@IdFamilia", id)))
                {
                    if (reader.Read())
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        familia = FamiliaMapper.Current.Fill(data);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener la Familia por Id.", ex);
            }

            return familia;
        }

        /// <summary>
        /// Obtiene todas las Familias del sistema.
        /// </summary>
        /// <returns>Una lista de todas las instancias de Familia.</returns>
        public List<Familia> GetAll()
        {
            List<Familia> familias = new List<Familia>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader("FamiliaSelectAll", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        familias.Add(FamiliaMapper.Current.Fill(data));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener todas las Familias.", ex);
            }

            return familias;
        }
    }
}