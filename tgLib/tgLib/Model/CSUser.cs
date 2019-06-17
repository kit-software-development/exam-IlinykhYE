using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGL.Model
{
    /// <summary>
    /// Реализация модели пользователя.
        /// </summary>
        /// Атрибут сериализации. Сериализация применяется к свойствам и полям класса
    [Serializable]
    public class CSUser
    {
        private string login; //логин
        private string pass; //пароль
        private int points; //кол очков
        private int nbParties; //кол сыгранных партий
        [NonSerialized]
        private int index;

        //переполнение
        public CSUser(string login)
        {
            this.Login = login;
            this.Points = 0;
            this.NbParties = 0;
        }

        public CSUser(string login, string pass)
        {
            this.Login = login;
            this.pass = pass;
            this.Points = 0;
            this.NbParties = 0;
        }

        public CSUser() { }

        //если выйграл, то +3 очка
        public void win()
        {
            points += 3;
        }
        //ничья +1 очко
        public void draw()
        {
            points += 1;
        }
        //добавление +1 после сыгранной партии
        public void incrementParties()
        {
            nbParties++;
        }
        //свойства
        public string Login { get { return login; } set { login = value; } }
        public string Pass { get { return pass; } set { pass = value; } }
        public int Points { get { return points; } set { points = value; } }
        public int NbParties { get { return nbParties; } set { nbParties = value; } }
        public int Index { get { return index; } set { index = value; } }
       
        //проверяем идентичность объектов по значению
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return login.Equals(((CSUser)obj).Login);
        }

        public override int GetHashCode()
        {
            return login.GetHashCode();
        }

        public override string ToString()
        {
            return login + " [ Points = " + points + ", Nb matches = " + nbParties + " ]";
        }
    }
}
