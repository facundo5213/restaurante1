using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain;


namespace SERVICE.Dao.Contracts
{
    public interface IRoleRepository
    {
        Acceso FindById(int id);
    }
}
