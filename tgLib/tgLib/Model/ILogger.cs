using System;
using System.Text;

namespace TGL.Model
{
    /// <summary>
    /// перечисление именнованых констант (спиок перечислителей)
    /// </summary>
    public enum Severiry { INFO, WARNING, ERROR };
    /// <summary>
    /// интерфейс авторизации
    /// </summary>
    public interface ILogger
    {
        void log(Severiry severity, string msg);
    }
}
