using Services.Dao;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Services.Logic
{
    /// <summary>
    /// Lógica de negocio para la traducción de claves de texto en diferentes idiomas.
    /// </summary>
    internal static class LanguageLogic
    {
        /// <summary>
        /// Traduce una clave especificada al texto correspondiente en el idioma actual.
        /// </summary>
        /// <param name="key">La clave a traducir.</param>
        /// <returns>El texto traducido asociado con la clave especificada.</returns>
        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("La clave no puede ser nula o estar vacía.", nameof(key));
            }

            try
            {
                return LanguageDao.Translate(key);
            }
            catch (KeyNotFoundException ex)
            {
                // Envía la clave no encontrada y la registra en los logs.
                LanguageDao.WriteKey(Thread.CurrentThread.CurrentUICulture.Name, key, $"[{key}]");
                LoggerDao.WriteLog(new Log($"La clave '{key}' no se encontró en el idioma '{Thread.CurrentThread.CurrentUICulture.Name}' y fue registrada.", TraceLevel.Warning), ex);
                return $"[{key}]"; // Retorna la clave entre corchetes para indicar que no se encontró.
            }
            catch (Exception ex)
            {
                // Registra cualquier otra excepción que ocurra.
                LoggerDao.WriteLog(new Log("Error al traducir la clave.", TraceLevel.Error), ex);
                return key; // Retorna la clave original en caso de error.
            }
        }

        /// <summary>
        /// Agrega una nueva clave y su valor a un archivo de idioma especificado.
        /// </summary>
        /// <param name="language">El idioma al que pertenece la clave.</param>
        /// <param name="key">La clave a agregar.</param>
        /// <param name="value">El valor de la clave a agregar.</param>
        public static void AddTranslation(string language, string key, string value)
        {
            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Idioma, clave o valor no pueden ser nulos o vacíos.");
            }

            try
            {
                LanguageDao.WriteKey(language, key, value);
                LoggerDao.WriteLog(new Log($"La clave '{key}' fue agregada al idioma '{language}' con el valor '{value}'.", TraceLevel.Info));
            }
            catch (Exception ex)
            {
                // Registra cualquier excepción que ocurra al agregar la clave.
                LoggerDao.WriteLog(new Log($"Error al agregar la clave '{key}' al idioma '{language}'.", TraceLevel.Error), ex);
            }
        }

        /// <summary>
        /// Recarga todos los archivos de idioma en la caché.
        /// </summary>
        public static void ReloadLanguages()
        {
            try
            {
                LanguageDao.ReloadLanguages();
                LoggerDao.WriteLog(new Log("Se recargaron todos los archivos de idioma en la caché.", TraceLevel.Info));
            }
            catch (Exception ex)
            {
                // Registra cualquier excepción que ocurra al recargar los idiomas.
                LoggerDao.WriteLog(new Log("Error al recargar los archivos de idioma.", TraceLevel.Error), ex);
            }
        }

        /// <summary>
        /// Obtiene una lista de todos los idiomas disponibles.
        /// </summary>
        /// <returns>Lista de identificadores de idiomas.</returns>
        public static List<string> GetLanguages()
        {
            try
            {
                return LanguageDao.GetLanguages();
            }
            catch (Exception ex)
            {
                // Registra cualquier excepción que ocurra al obtener los idiomas.
                LoggerDao.WriteLog(new Log("Error al obtener la lista de idiomas.", TraceLevel.Error), ex);
                return new List<string>(); // Retorna una lista vacía si ocurre un error.
            }
        }
    }
}
