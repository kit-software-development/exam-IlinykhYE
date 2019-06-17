using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGL.Model
{
    /// <summary>
    /// Управление чтением / записью данных пользователя в базе данных через соединение OleDb.
    /// </summary>
    public class OleDbManager : DataManager
    {
        //создание соединения
        private OleDbConnection con;

        public OleDbManager(string conString)
        {
            try
            {
                con = new OleDbConnection(conString);
                con.Open();
            }catch(Exception e){
                //ошибка соединения с бд
                throw new Exception("Error occurred when connecting to the database : " + e.Message);
            }
        }
        //закрытие соединения
        public override void close()
        {
            con.Close();
        }
        //создание пользователя в бд
        public override void create(CSUser user)
        {
            if (user == null) throw new Exception("CSUser cannot be null !");
            try
            {
                OleDbCommand cmdInsert = new OleDbCommand();
                cmdInsert.Connection = con;
                cmdInsert.CommandText = "INSERT INTO [user] (login, pass, points, nbparties) VALUES(@login, @pass, @points, @nbparties)";
                cmdInsert.Parameters.Add("@login", OleDbType.VarChar, 20).Value = user.Login;
                cmdInsert.Parameters.Add("@pass", OleDbType.VarChar, 20).Value = sha1Encrypt(user.Pass);
                cmdInsert.Parameters.Add("@points", OleDbType.Integer,  11).Value = user.Points;
                cmdInsert.Parameters.Add("@nbparties", OleDbType.Integer,  11).Value = user.NbParties;
                cmdInsert.Prepare();
                cmdInsert.ExecuteNonQuery();//выполнить
            }
            //ошибка
            catch (Exception)
            {
                throw new Exception("Error when inserting user '"+user.Login+"' !");
            }
        }
        //изменение
        public override void update(CSUser user)
        {
            if (user == null) throw new Exception("CSUser cannot be null !");
            try
            {
                OleDbCommand cmdUpdate = new OleDbCommand();
                cmdUpdate.Connection = con;
                cmdUpdate.CommandText = "UPDATE [user] SET points = @points, nbparties = @nbparties WHERE login = @login";
                cmdUpdate.Parameters.Add("@points", OleDbType.Integer, 11).Value = user.Points;
                cmdUpdate.Parameters.Add("@nbparties", OleDbType.Integer, 11).Value = user.NbParties;
                cmdUpdate.Parameters.Add("@login", OleDbType.VarChar, 20).Value = user.Login;
                cmdUpdate.Prepare();
                cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw new Exception("Error when updating user '"+user.Login+"' !");
            }
        }
        //поиск
        public override CSUser find(CSUser user)
        {
            if (user == null) throw new Exception("CSUser cannot be null !");
            try
            {
                OleDbCommand cmdFind = new OleDbCommand();
                cmdFind.Connection = con;
                cmdFind.CommandText = "SELECT login, points, nbparties FROM [user] WHERE login = @login AND pass = @pass";
                cmdFind.Parameters.Add("@login", OleDbType.VarChar, 25).Value =  user.Login;
                cmdFind.Parameters.Add("@pass", OleDbType.VarChar, 25).Value = sha1Encrypt(user.Pass);
                cmdFind.Prepare();
                //ExecuteReader нужен для считывания данных, которые хранятся в таблице
                OleDbDataReader reader = cmdFind.ExecuteReader();
                CSUser logUser = null;
            
                if (reader.Read())
                {
                    logUser = new CSUser();
                    logUser.Login = reader.GetString(0);
                    logUser.Points = reader.GetInt32(1);
                    logUser.NbParties = reader.GetInt32(2);
                }
                reader.Close();
                return logUser;
            }
            catch (Exception e)
            {
                throw new Exception("Error when find user '" + user.Login + "' : " + e.Message);
            }
        }
        //проверка на существование логина в таблице
        public override bool exist(string login)
        {
            //Указывает, имеет ли указанная строка значение null, 
            //является ли она пустой строкой или строкой, состоящей только из символов-разделителей
            if (String.IsNullOrWhiteSpace(login)) return false;
            try
            {
                OleDbCommand cmdExist = new OleDbCommand();
                cmdExist.Connection = con;
                cmdExist.CommandText = "SELECT login FROM [user] WHERE login = @login";
                cmdExist.Parameters.Add("@login", OleDbType.VarChar, 20).Value = login;
                cmdExist.Prepare();

                OleDbDataReader reader = cmdExist.ExecuteReader();
                bool exists = reader.Read();
                reader.Close();
                return exists;
            }
            catch (Exception)
            {
                //Ошибка при проверке существующего пользователя
                throw new Exception("Error when check existing user !");
            }
        }

        public override string ToString()
        {
            return "OleDB";
        }
    }
}
