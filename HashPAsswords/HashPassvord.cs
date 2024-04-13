using System;
using System.Security.Cryptography;
using System.Text;


namespace HashPAsswords
{
    /// <summary>
    /// Данный класс предназначен для хэширования пароля
    /// </summary>
    public class HashPassvord
    {
        /// <summary>
        /// Метод принимает пароль в строковом представлнии
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Вернет строку в хэшированном виде</returns>
        public static string HashPassword(string password) 
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceBytePassw = Encoding.UTF8.GetBytes(password);
                byte[] hashSourceBytePassw = sha256Hash.ComputeHash(sourceBytePassw);
                string hashPassw = BitConverter.ToString(hashSourceBytePassw).Replace("-", String.Empty);
                return hashPassw;
            }
        }
    }
}
