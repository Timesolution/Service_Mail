using Service_Mail.Capa_Entidades;
using Service_Mail.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service_Mail.Negocio
{
    public class Service
    {
        public void Working()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Inicio de correr tareas");

                while (true)
                {
                    DateTime fechaActual = DateTime.Now;
                    DateTime fechaCorrer = Settings.Default.Hora_Correr;

                    if (fechaActual.Hour == fechaCorrer.Hour &&
                        fechaActual.Minute == fechaCorrer.Minute)
                    {

                        int dias = 3;

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Busco la configuracion de dias");

                        Gestion_Api.Modelo.Configuracion configuracion = new Gestion_Api.Modelo.Configuracion();

                        if (!string.IsNullOrWhiteSpace(configuracion.DiasRecordatorioMail))
                        {
                            dias = Convert.ToInt32(configuracion.DiasRecordatorioMail);
                        }

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Encuentro la configuracion de dias: " + dias);

                        Estetica_Api.Controladores.ControladorAgenda controladorAgenda = new Estetica_Api.Controladores.ControladorAgenda();

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Busco lo agendado");

                        var agendas = controladorAgenda.ObtenerAgendasEnDias(dias);

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Encuentro agendados: " + agendas.Count);

                        if (agendas.Count > 0)
                        {
                            Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Inicio el envio de mails");

                            foreach (var agenda in agendas)
                            {
                                int enviado = controladorAgenda.EnviarMailPacienteRecordatorio(agenda, Settings.Default.Path_Logo, Settings.Default.Link_Confirmacion + agenda.IdAgenda);

                                if(enviado == 1)
                                {
                                    Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el mail");
                                }
                                else
                                {
                                    Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El mail no se envio correctamente");
                                }
                            }

                        }

                        //espero un minuto
                        Thread.Sleep(60000);

                    }

                    //espero un segundo
                    Thread.Sleep(1000);

                }

            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLog(DateTime.Now + " ERROR(CATCH): Ocurrio un error en Service.Working. Excepcion: " + ex.Message);
            }
        }

    }
}
