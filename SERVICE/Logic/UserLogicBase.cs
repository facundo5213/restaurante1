using Services.Domain;  // Asegúrate de que el espacio de nombres sea correcto
using Services.Dao.Implementations.SqlServer;  // Si estás usando un singleton en el repositorio
using System;

namespace Services.Logic
{
    internal static class UserLogicBase
    {
        /// <summary>
        /// Valida las credenciales de un usuario.
        /// </summary>
        /// <param name="user">El usuario a validar.</param>
        /// <returns>Retorna true si las credenciales son válidas; de lo contrario, false.</returns>
        public static bool Validate(Usuario user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
            }

            // Verificar el nombre de usuario y la contraseña contra la base de datos
            Usuario usuarioDB = UsuarioRepository.Current.GetById(user.IdUsuario); // Asegúrate de que Current esté implementado correctamente

            if (usuarioDB != null && usuarioDB.UserName == user.UserName && usuarioDB.Password == user.Password)
            {
                // Aquí puedes cambiar por un log o eliminar el Console.WriteLine en aplicaciones que no son de consola.
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
