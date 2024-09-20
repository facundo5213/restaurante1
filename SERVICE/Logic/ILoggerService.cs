using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Logic
{
    public interface ILoggerService
    {
        void Log(string message);  // Registra un mensaje de log
    }
}
