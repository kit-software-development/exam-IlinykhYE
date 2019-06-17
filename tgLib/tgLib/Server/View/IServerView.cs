using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGL.Model;

namespace TGL.Server.View
{
    public delegate void newStartServerHandler(int port);
    public delegate void newStopServerHandler();

    public interface IServerView
    {
        event newStartServerHandler startServerHandler;
        event newStopServerHandler stopServerHandler;

        /// <summary>
        /// Показать представление сервера.
        /// </summary>
        void showView();
        /// <summary>
        ///  Return true если сервер вкл
        /// </summary>
        /// <returns></returns>
        bool isOpened();
        /// <summary>
        /// очистить
        /// </summary>
        void clearAll();
        /// <summary>
        /// добавление пользователя
        /// </summary>
        /// <param name="user"></param>
        void addUser(CSUser user);
        /// <summary>
        /// соед и изм пользователя
        /// </summary>
        /// <param name="login">Login connection</param>
        /// <param name="user"></param>
        void update(string login, CSUser user);
        /// <summary>
        /// удаление пользователя
        /// </summary>
        /// <param name="user"></param>
        void delete(CSUser user);
        /// <summary>
        /// старт новой игры между двумя игроками
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        void newParty(CSUser user1, CSUser user2);
        /// <summary>
        /// удаление с сервера игры
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        void removeParty(CSUser user1, CSUser user2);
        /// <summary>
        /// обновление состояния сервера
        /// </summary>
        /// <param name="state"></param>
        void updateState(bool state);
        /// <summary>
        /// вывод сообщения
        /// </summary>
        /// <param name="msg"></param>
        void printMessage(KeyValuePair<Severiry, string> msg);
    }
}
