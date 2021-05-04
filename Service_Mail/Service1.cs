using Service_Mail.Capa_Entidades;
using Service_Mail.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service_Mail
{
    public partial class Service1 : ServiceBase
    {
        public Thread MainPrimario = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Service service = new Service();

                MainPrimario = new Thread(new ThreadStart(service.Working));
                MainPrimario.Start();
            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLog(DateTime.Now + " ERROR(CATCH): Ocurrio un error en OnStart. Excepcion: " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            MainPrimario.Abort();
        }
    }
}
