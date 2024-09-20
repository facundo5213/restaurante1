using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;



namespace SERVICE.Dao.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Valida si una contraseña ingresada coincide con la contraseña encriptada.
        /// </summary>
        /// <param name="plainPassword">La contraseña ingresada en texto plano.</param>
        /// <param name="hashedPassword">La contraseña encriptada almacenada en la base de datos.</param>
        /// <returns>True si coinciden, false de lo contrario.</returns>
        public static bool ValidatePassword(string plainPassword, string hashedPassword)
        {
            // Hashear la contraseña ingresada
            string hashedInput = EncryptPassword(plainPassword);
            return hashedInput == hashedPassword;
        }

        /// <summary>
        /// Encripta una contraseña en texto plano utilizando SHA256.
        /// </summary>
        /// <param name="password">La contraseña en texto plano a encriptar.</param>
        /// <returns>La contraseña encriptada en formato hexadecimal.</returns>
        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
