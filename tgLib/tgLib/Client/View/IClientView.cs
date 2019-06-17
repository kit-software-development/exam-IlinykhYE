using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using TGL.Model;
using TGL.Client.Model;

namespace TGL.Client.View
{
    public delegate void newConnectionEvent(TcpClient tcpclient);
    public delegate void newDeconnectionEvent();
    public delegate void newLoginEvent(CSUser user, bool enroll, bool save);
    public delegate void newAbortEvent(CSUser user, bool sent);
    public delegate void newRequestEvent(CSUser user);
    public delegate void newResponseEvent(CSUser user);
    public delegate void newQuitGameEvent();
    public delegate void newEndGameEvent(Object gameStuff, GameState state);
    public delegate void newSendMsgEvent(string msg);
    public delegate void newGameEvent(Object gameStuff);

    /// <summary>
    /// Client view interface.
    /// </summary>
    public interface IClientView
    {
        event newConnectionEvent connectionEvent;
        event newDeconnectionEvent deconnectionEvent;
        event newLoginEvent loginEvent;
        event newAbortEvent abortEvent;
        event newRequestEvent requestEvent;
        event newResponseEvent responseEvent;
        event newQuitGameEvent quitGameEvent;
        event newSendMsgEvent sendMsgEvent;
        event newEndGameEvent endGameEvent;
        event newGameEvent gameEvent;

        /// <summary>
        /// показать клиента
        /// </summary>
        /// <param name="user"></param>
        void showView(CSUser user);
        /// <summary>
        /// возврат true если клиент найден
        /// </summary>
        bool isOpened();
        /// <summary>
        /// показать параметры регистрации, если текущий пользователь не зарегистрирован.
        /// </summary>
        void showRegisterOption();
        /// <summary>
        /// Обработка события отключения. Обновление того, как показан клиент
        /// </summary>
        void deconnectionHandler();
        /// <summary>
        /// Обработка события подключения
        /// </summary>
        /// <param name="user"></param>
        void connectionHandler(CSUser user);
        /// <summary>
        /// Обработка события входа в систему, обновление представления клиента
        /// </summary>
        /// <param name="user">Logged user</param>
        void loginHandler(CSUser user);
        /// <summary>
        /// обработка события отмены, поступающего от какого-либо пользователя
        /// </summary>
        /// <param name="user">Пользователь, который прервал игру</param>
        /// <param name="sent">True, если сообщение об отмене уже отправлено </param>
        void abortHandler(CSUser user, bool sent);
        /// <summary>
        /// Обработка полученного игрового запроса от пользователя
        /// </summary>
        /// <param name="user">Пользователь, который просит игры</param>
        void requestHandler(CSUser user);
        /// <summary>
        /// Обработка события начала игры
        /// </summary>
        /// <param name="user">тот, кому отправили запрос</param>
        /// <param name="play">True, если текущий пользователь должен играть первым</param>
        void startGameHandler(CSUser user, bool play);
        /// <summary>
        /// Обработка события остановки игры
        /// </summary>
        /// <param name="user">кто, остановил</param>
        void stopGameHandler(CSUser user);
        /// <summary>
        /// Обработка игрового события с полученными данными.
        /// </summary>
        /// <param name="gameStuff">Полученные данные игры</param>
        void gameHandler(Object gameStuff);
        /// <summary>
        /// Обработка события окончания игры
        /// </summary>
        /// <param name="user">текущий пользователь</param>
        /// <param name="challenger"></param>
        /// <param name="state"></param>
        /// <param name="play"></param>
        /// <param name="gameStuff"></param>
        void endGameHandler(CSUser user, CSUser challenger, GameState state, bool play, Object gameStuff);
        /// <summary>
        /// Обработка полученного сообщения от данного пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="msg"></param>
        void receiveMsgHandler(CSUser user, string msg);
        /// <summary>
        /// Обработка события списка пользователей
        /// </summary>
        /// <param name="users">Новый список подключенных пользователей</param>
        void userListHandler(List<CSUser> users);
        /// <summary>
        /// событие очистки
        /// </summary>
        void clearHandler();
        /// <summary>
        /// Вывод сообщения на доску
        /// </summary>
        /// <param name="msg"></param>
        void printCSMessage(KeyValuePair<Severiry, string> msg);
    }
}
