using SERVICE.Logic;
using Services.Dao;
using Services.Domain;
using System;
using System.Diagnostics;

namespace Services.Logic
{
    /// <summary>
    /// Lógica para el manejo de registros (logs) en la aplicación.
    /// </summary>
    public class LoggerLogic : ILoggerService
    {
        /// <summary>
        /// Escribe un registro de log en el sistema.
        /// </summary>
        /// <param name="message">El mensaje de log a registrar.</param>
        public void Log(string message)
        {
            Console.WriteLine($"Log: {message}");
        }
    }
}

