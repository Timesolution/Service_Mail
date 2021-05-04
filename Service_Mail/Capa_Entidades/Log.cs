using System.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Mail.Capa_Entidades
{
    public sealed class Log
    {
        static readonly object padlock = new object();
        static Log _instance = null;

        public static Log Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Log();
                    }
                    return _instance;
                }
            }
        }

        public void EscribirEnLog(string message)
        {
            try
            {

                StreamWriter streamWriter = new StreamWriter(ConfigurationManager.AppSettings["RutaLog"] , true);

                streamWriter.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy") + "]" + message);

                streamWriter.Close();
            }
            catch (Exception ex)
            {
                StreamWriter streamWriter = new StreamWriter(ConfigurationManager.AppSettings["RutaLog"], true);

                streamWriter.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy") + "]" + "ERROR: Ocurrio un error en Log.EscribirEnLog().Excepcion: " + ex.Message);

                streamWriter.Close();
            }
        }


    }
}
