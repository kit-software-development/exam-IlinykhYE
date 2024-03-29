﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TGL.Model;
using TGL.Client.Controller;
using TGL.Client.View;

namespace GCM
{
    /// <summary>
    /// Connect4 Application. 
    /// пример использования библиотеки на стороне клиента
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FileManager manager = new FileManager(false);
                IClientView view = new view.Connect4Form();
                ClientController controller = new ClientController(manager, view);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
