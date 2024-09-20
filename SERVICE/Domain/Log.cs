using System;
using System.Diagnostics;

namespace Services.Domain
{
    /// <summary>
    /// Representa una entrada de log para el monitoreo y depuración del sistema.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Mensaje de la entrada de log.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Nivel de severidad de la entrada de log.
        /// </summary>
        public TraceLevel TraceLevel { get; set; }

        /// <summary>
        /// Marca de tiempo cuando se creó la entrada de log.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase Log.
        /// </summary>
        /// <param name="message">El mensaje del log.</param>
        /// <param name="traceLevel">El nivel de severidad del log (por defecto es TraceLevel.Info).</param>
        /// <param name="date">La marca de tiempo de la entrada del log (por defecto es la hora actual).</param>
        public Log(string message, TraceLevel traceLevel = TraceLevel.Info, DateTime date = default)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message), "El mensaje del log no puede ser nulo.");
            TraceLevel = traceLevel;
            Date = (date == default) ? DateTime.Now : date;
        }

        /// <summary>
        /// Devuelve una representación formateada en cadena de la entrada del log.
        /// </summary>
        /// <returns>Una cadena que representa el objeto log actual.</returns>
        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd HH:mm:ss} [{TraceLevel}]: {Message}";
        }
    }
}
