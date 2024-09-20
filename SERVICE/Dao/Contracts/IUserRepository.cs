using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain;

namespace SERVICE.Dao.Contracts
{
    public interface IUserRepository
    {
        Usuario FindByUsername(string username);
        Usuario FindById(Guid id);
        void Add(Usuario user);
        IEnumerable<Usuario> GetAll();
    }
}
