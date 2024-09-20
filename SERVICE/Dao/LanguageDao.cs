using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Services.Dao
{
    internal static class LanguageDao
    {
        // Obtén la ruta del archivo de configuración App.config
        private static readonly string path = ConfigurationManager.AppSettings["LanguagePath"];

        // Diccionario para almacenar las traducciones en caché
        private static Dictionary<string, Dictionary<string, string>> cache = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Traduce una clave especificada al texto correspondiente en el idioma actual.
        /// </summary>
        /// <param name="key">La clave a traducir.</param>
        /// <returns>El texto traducido asociado con la clave especificada.</returns>
        public static string Translate(string key)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;  // Idioma actual (ej: "es-ES")
            string fileName = Path.Combine(path, $"{language}.txt");

            // Verifica si las traducciones ya están en caché
            if (!cache.ContainsKey(language))
            {
                // Cargar las traducciones en caché si no se ha hecho
                LoadLanguageFile(language, fileName);
            }

            // Intenta obtener la traducción desde la caché
            if (cache[language].TryGetValue(key.ToLower(), out string translation))
            {
                return translation;
            }

            // Si la clave no se encuentra, regresa la clave original entre corchetes
            return $"[{key}]";
        }

        /// <summary>
        /// Carga y parsea el archivo de idioma en la caché.
        /// </summary>
        /// <param name="language">Identificador del lenguaje (ej: "es-ES").</param>
        /// <param name="fileName">Nombre del archivo a cargar.</param>
        private static void LoadLanguageFile(string language, string fileName)
        {
            var translations = new Dictionary<string, string>();

            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        {
                            continue; // Saltar líneas vacías o comentarios
                        }

                        string[] columns = line.Split('=');
                        if (columns.Length == 2)
                        {
                            translations[columns[0].Trim().ToLower()] = columns[1].Trim();
                        }
                    }
                }

                // Almacenar las traducciones en caché
                cache[language] = translations;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"Archivo de idioma '{fileName}' no encontrado.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar el archivo de idioma '{fileName}': {ex.Message}");
            }
        }

        /// <summary>
        /// Agrega una nueva clave y su valor al archivo de idioma especificado.
        /// </summary>
        /// <param name="language">El idioma al que pertenece la clave.</param>
        /// <param name="key">Clave a escribir.</param>
        /// <param name="value">Valor de la clave a escribir.</param>
        public static void WriteKey(string language, string key, string value)
        {
            string fileName = Path.Combine(path, $"{language}.txt");

            // Asegurarse de que el archivo y la caché estén cargados
            if (!cache.ContainsKey(language))
            {
                LoadLanguageFile(language, fileName);
            }

            // Escribir la clave en la caché
            cache[language][key.ToLower()] = value;

            // Escribir la clave en el archivo de idioma
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine($"{key}={value}");
            }
        }

        /// <summary>
        /// Obtiene una lista de todos los idiomas disponibles.
        /// </summary>
        /// <returns>Lista de identificadores de idiomas (ej: "es-ES", "en-US").</returns>
        public static List<string> GetLanguages()
        {
            List<string> languages = new List<string>();

            try
            {
                foreach (var file in Directory.GetFiles(path, "*.txt"))
                {
                    languages.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista de idiomas: {ex.Message}");
            }

            return languages;
        }

        /// <summary>
        /// Recarga los archivos de idioma en la caché.
        /// </summary>
        public static void ReloadLanguages()
        {
            cache.Clear();

            foreach (var language in GetLanguages())
            {
                string fileName = Path.Combine(path, $"{language}.txt");
                LoadLanguageFile(language, fileName);
            }
        }
    }
}