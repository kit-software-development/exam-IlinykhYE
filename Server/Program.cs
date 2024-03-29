﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TGL.Server.View;
using TGL.Model;
using TGL.Server.Controller;

namespace GSM
{
    /// <summary>
    /// Серверная часть.Используется tgLib 
    /// Здесь мы используем соединение OleDb для доступа к БД доступа.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string provider = "Microsoft.ACE.OLEDB.12.0";
            string datasource = System.Environment.CurrentDirectory + "\\users.accdb";
            string userid = "";
            string password = "";
            string conString ="Provider=" + provider + ";Data Source=" +datasource 
                +"; User ID=" + userid + ";Password=" + password +";";
            
            try {
                IServerView view = new view.ServerForm();
                DataManager manager = new OleDbManager(conString);
                ServerController ctl = new ServerController(view);
            }
            catch (Exception e) { 
                Console.WriteLine(e.Message);
            }
        }
    }
}
