using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGL.Client.Model
{
    /// Перечень состояний игры
    public enum GameState { WON, LOST, DRAW, CONTINUE}
    /// Game main class.
    public abstract class Game
    {
        public abstract void reset();
        public abstract void setLastMove(Object move, int player);
        public abstract GameState updateMove();
    }
}
