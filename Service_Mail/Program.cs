﻿using Service_Mail.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Service_Mail
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //Test estetica
            //Service servicio = new Service();
            //servicio.Working();
            //Test Mascotas agenda
            //Service servicio = new Service();
            //servicio.Working2();
            //Test Mascotas Historiaclinica
            //Service servicio = new Service();
            //servicio.Working3();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
