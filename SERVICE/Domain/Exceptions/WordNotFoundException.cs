using System;
using Services.Facade;
using Services.Facade.Extentions;

namespace Services.Domain.Exceptions
{
    internal class WordNotFoundException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase WordNotFoundException con un mensaje por defecto.
        /// </summary>
        public WordNotFoundException() : base("No se encontró".Translate())
        {
            NotifyAdmin();
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase WordNotFoundException con la palabra que no fue encontrada.
        /// </summary>
        /// <param name="word">La palabra que no pudo ser encontrada.</param>
        public WordNotFoundException(string word) : base($"La palabra '{word}' no fue encontrada.".Translate())
        {
            NotifyAdmin(word);
        }

        /// <summary>
        /// Envía una notificación al grupo de administración (vía WhatsApp o por otros medios).
        /// </summary>
        /// <param name="word">La palabra que provocó la excepción.</param>
        private void NotifyAdmin(string word = null)
        {
            string message = word != null
                ? $"Alerta: La palabra '{word}' no fue encontrada."
                : "Alerta: Se lanzó una WordNotFoundException.";

            // Aquí podrías integrar una lógica para enviar un mensaje al grupo de administración.
            // Ejemplo: WhatsAppService.SendMessage("NúmeroDelGrupo", message);
            Console.WriteLine(message); // Simulación de envío de mensaje.
        }
    }
}
