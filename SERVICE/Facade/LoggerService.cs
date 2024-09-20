﻿using Services.Domain;
using Services.Logic;
using System;
using System.Diagnostics;

namespace Services.Facade
{
    /// <summary>
    /// Proporciona una interfaz simplificada para realizar operaciones de registro (logging) en la aplicación.
    /// </summary>
    public static class LoggerService
    {
        /// <summary>
        /// Escribe un registro de log en el sistema, con opción de incluir detalles de una excepción.
        /// </summary>
        /// <param name="log">Objeto de log que contiene la información a registrar.</param>
        /// <param name="ex">Excepción opcional cuyos detalles se incluirán en el log.</param>
        public static void WriteLog(Log log, Exception ex = null)
        {
            // Llamada a la lógica para escribir el log
            LoggerLogic.WriteLog(log, ex);
        }

        /// <summary>
        /// Escribe un registro de log utilizando solo un mensaje y un nivel de severidad.
        /// El nivel de severidad por defecto es INFO.
        /// </summary>
        /// <param name="message">Mensaje de log a registrar.</param>
        /// <param name="level">Nivel de severidad del log (por defecto es TraceLevel.Info).</param>
        public static void WriteLog(string message, TraceLevel level = TraceLevel.Info)
        {
            // Crear un objeto de log y escribirlo
            LoggerLogic.WriteLog(new Log(message, level));
        }

        /// <summary>
        /// Registra una excepción en el sistema como un error crítico.
        /// </summary>
        /// <param name="ex">Excepción a registrar.</param>
        public static void WriteException(Exception ex)
        {
            // Registrar la excepción como un error crítico
            LoggerLogic.WriteLog(new Log(ex.Message, TraceLevel.Error), ex);
        }
    }

    /// <summary>
    /// Clase que representa un registro de log.
    /// </summary>
    public class Log
    {
        public string Message { get; set; }
        public TraceLevel Level { get; set; }
        public DateTime Timestamp { get; set; }

        public Log(string message, TraceLevel level)
        {
            Message = message;
            Level = level;
            Timestamp = DateTime.Now;
        }
    }
}