using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TGL.Model
{
    public abstract class UserThread : ILogger
    {
        private CSUser user;
        private CSUser to;
        private List<CSUser> froms = new List<CSUser>();

        public const string DEFAULTNAME = "ANONYMOUS";
        //Для многопоточности
        private Thread thread;

        //Для создания клиентской программы, работающей по протоколу TCP
        private TcpClient tcpclient;

        //Для сериализации (преобразования объекта в поток байтов)
        private IFormatter formatter;

        //Если возникнет необходимость прервать выполняемую задачу
        private CancellationTokenSource cts;

        private readonly object sync = new object();

        public delegate void newEndGameHandler(UserThread ut, CSMessage msg);
        public delegate void newStopGameHandler(UserThread ut, CSMessage msg);
        public delegate void newLoginHandler(UserThread ut, CSMessage msg);
        public delegate void newChallengerHandler(UserThread ut, CSMessage msg);
        public delegate void newDeconnectionHandler(UserThread ut);

        //KeyValuePair - словарь. Удобно обращаться к Severiry { INFO, WARNING, ERROR }
        public delegate void newMessageHandler(KeyValuePair<Severiry, string> msg);

        public newEndGameHandler endGameHandler;
        public newStopGameHandler stopGameHandler;
        public newLoginHandler loginHandler;
        public newChallengerHandler challengerHandler;
        public newMessageHandler messageHandler;
        public newDeconnectionHandler deconnectionHandler;

        //создание потока инициализации клиента в табл результата
        public UserThread(TcpClient tcpclient)
        {
            this.tcpclient = tcpclient;
            //BinaryFormatter сериализует состояние объекта в поток, 
            //используя компактный двоичный формат
            this.formatter = new BinaryFormatter();
            this.user = new CSUser(DEFAULTNAME);
        }

        public List<CSUser> Froms { get { return froms; } }
        public CSUser ClientUser { get { return user; } set { user = value; } }
        public CSUser To { get { return to; } set { to = value; } }

        /// <summary>
        /// Проверка. Зарегестрирован ли пользователь
        /// </summary>
        public bool isLogged()
        {
            //Определяет, совпадает ли начало данного экземпляра строки имени с указанной строкой.
            return ! user.Login.StartsWith(DEFAULTNAME);
        }
        /// <summary>
        /// проверка. Запущен ли текущий процесс
        /// </summary>
        public bool isStarted()
        {
            //если IsCancellationRequested = true, то задача должна остановиться
            return cts != null && !cts.IsCancellationRequested;
        }
        /// <summary>
        /// Запуск текущего процесса
        /// </summary>
        public void start()
        {
            try
            {
                //определяет блок кода, внутри которого весь код блокируется 
                //и становится недоступным для других потоков до завершения работы текущего потока
                lock (sync)
                {
                    //проверка, если поток не активен||пустой, создание потока
                    if (tcpclient != null && (thread == null || !thread.IsAlive)) //IsAlive-возвращает true, если поток активен
                    {
                        //создание объекта, который создает токен отмены и запрос на отмену для всех копий этого токена.
                        cts = new CancellationTokenSource();
                        thread = new Thread(() => run(cts.Token))
                        {
                            IsBackground = true
                        };
                        thread.Start();
                    }
                    else
                        //если уже запущен, то выдает сообщение, что уже запущен, и произошла попытка перезагрузки
                        log(Severiry.WARNING, "Attempt to restart when it is already started !");
                }
            }
            //обработка исключения
            catch (Exception e) 
            {
                log(Severiry.ERROR, "Starting -> " + e.Message);
            }
        }
        /// <summary>
        /// Остановка текущего процесса
        /// </summary>
        public void stop()
        {
            try
            {
                if (tcpclient.Connected && thread != null && thread.IsAlive)
                {
                    lock (sync)
                    {
                        cts.Cancel();
                        tcpclient.Close();
                        log(Severiry.INFO, "Stopped");
                    }
                }
            }
            catch (Exception e)
            {
                log(Severiry.ERROR, "Stopping -> " + e.Message);
            }
        }

        public void log(Severiry severity, string msg)
        {
            messageHandler(new KeyValuePair<Severiry, string>(severity, "Client[" + user.Login + "] - " + msg));
        }
        /// <summary>
        /// написать сообщение
        /// </summary>
        
        public void send(CSMessage msg)
        {
            try
            {
                // используем для отправки и получения данных.
                formatter.Serialize(tcpclient.GetStream(), msg);
            }
            catch (Exception e)
            {
                log(Severiry.ERROR, "Sending -> " + e.Message);
            }
        }
        /// <summary>
        /// Метод запуска процесса.
        /// </summary>
        public void run(CancellationToken token)
        {
            //проверка текущего состояния соединения
            while (tcpclient.Connected && !token.IsCancellationRequested)
            {
                try
                {
                    CSMessage msg = (CSMessage)formatter.Deserialize(tcpclient.GetStream());
                    //Идентифицируем тип сообщения
                    switch (msg.Type)
                    {
                        case MessageType.DECONNECTION:
                            deconnetion();
                            break;
                        case MessageType.LOGIN:
                            loginHandler(this, msg);
                            break;
                        case MessageType.CHALLENGING:
                            challengerHandler(this, msg);
                            break;
                        case MessageType.STOP_GAME:
                            stopGameHandler(this, msg);
                            break;
                        case MessageType.END_GAME:
                            endGameHandler(this, msg);
                            break;
                        default:
                            handleMessage(msg);
                            break;
                    }

                }
                catch (Exception)
                {
                    if (!token.IsCancellationRequested) deconnetion();
                }
            }
        }
        /// <summary>
        /// Остановка текущего процесса
        /// </summary>
        public void deconnetion()
        {
            stop();
            deconnectionHandler(this);
        }

        public abstract void handleMessage(CSMessage msg);
    }
}
