using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TGL.Model
{
    /// <summary>
    /// пользователи
    /// </summary>
    public abstract class DataManager
    {
        /// <summary>
        /// создание пользователя
        /// </summary>
        public abstract void create(CSUser user);
        /// <summary>
        /// изменение пользователя
        /// </summary>
        public abstract void update(CSUser user);
        /// <summary>
        /// поиск пользователя
        /// </summary>
        public abstract CSUser find(CSUser user);
        /// <summary>
        /// проверка на существования логина в таблице
        /// </summary>
        public abstract bool exist(string login);

        /// <summary>
        /// закрытие.
        /// </summary>
        public abstract void close();

        /// <summary>
        /// шифрование пароля
        /// </summary>
        protected string sha1Encrypt(string pass)
        {
            using (var sha1 = SHA1.Create())
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(pass);
                    //Вычисляет хэш-значение для входных данных.
                    byte[] hashBytes = sha1.ComputeHash(bytes);

                    return Encoding.UTF8.GetString(hashBytes);
                }
                catch (Exception)
                {
                    //Ошибка при шифровании пароля
                    throw new Exception("Error when encryting password");
                }
            }
        }
    }
}
