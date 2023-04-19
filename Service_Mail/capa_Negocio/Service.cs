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
using W_Twilio;

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
                    Gestion_Api.Controladores.ControladorSMS controladorSMS = new Gestion_Api.Controladores.ControladorSMS();


                    if (fechaActual.Hour == fechaCorrer.Hour &&
                        fechaActual.Minute == fechaCorrer.Minute)
                    {
                     
                        int dias = 1;

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

                                if (enviado == 1)
                                {
                                    Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el mail");
                                }
                                else
                                {
                                    Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El mail no se envio correctamente");
                                }

                                if (!string.IsNullOrEmpty(configuracion.EnviarSMSRecordatorio))
                                {
                                    if (agenda.Propietarios != null && !String.IsNullOrEmpty(agenda.Propietarios.Celular))
                                    {
                                        string[] celular = agenda.Propietarios.Celular.Split('-');

                                        if (celular.Length == 2 && !String.IsNullOrEmpty(agenda.Propietarios.Celular) && celular[1].Length == 10)
                                        {
                                            string numero = celular[1];
                                            string codArea = celular[0];

                                            string telefono = "+549" + numero;

                                            int enviadoSMS = controladorSMS.enviarSMS(telefono, $"Recordatorio de turno: { agenda.Fecha?.ToString("dd/MM/yyyy") + " "  + agenda.HoraDesde.ToString() }  - { agenda.TiposEvento.Descripcion } - { agenda.Profesionales.NombreProfesional } { configuracion.NombreFantasiaSMS }", -1);
                                            if (enviadoSMS == 1)
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                            }
                                            else
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                            }
                                        }
                                        else if(agenda.Propietarios.Celular.Length == 10)
                                        {
                                            string telefono = "+549" + agenda.Propietarios.Celular;

                                            int enviadoSMS = controladorSMS.enviarSMS(telefono, $"Recordatorio de turno: { agenda.Fecha?.ToString("dd/MM/yyyy") + " " + agenda.HoraDesde.ToString() }  - { agenda.TiposEvento.Descripcion } - { agenda.Profesionales.NombreProfesional } { configuracion.NombreFantasiaSMS }", -1);
                                            if (enviadoSMS == 1)
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                            }
                                            else
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                            }

                                        }
                                        else if(agenda.Propietarios.Celular.Trim().Replace("-","").Replace(" ","").Length == 10)
                                        {
                                            string telefono = "+549" + agenda.Propietarios.Celular.Trim().Replace("-","").Replace(" ","");

                                            int enviadoSMS = controladorSMS.enviarSMS(telefono, $"Recordatorio de turno: { agenda.Fecha?.ToString("dd/MM/yyyy") + " " + agenda.HoraDesde.ToString() }  - { agenda.TiposEvento.Descripcion } - { agenda.Profesionales.NombreProfesional } { configuracion.NombreFantasiaSMS }", -1);
                                            if (enviadoSMS == 1)
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                            }
                                            else
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                            }
                                        }

                                    }
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
        public void Working2()
        {
            try
            {



                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Inicio de correr tareas");

                while (true)
                {
                    DateTime fechaActual = DateTime.Now;
                    DateTime fechaCorrer = Settings.Default.Hora_Correr;
                    Gestion_Api.Controladores.ControladorSMS controladorSMS = new Gestion_Api.Controladores.ControladorSMS();
                    ControladorTwilio ControladorTwilioWhatsapp = new ControladorTwilio();
                    bool WhatsAppHabilitado = false;
                    if (fechaActual.Hour == fechaCorrer.Hour &&
                        fechaActual.Minute == fechaCorrer.Minute)
                    {

                        int dias = 1;

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Busco la configuracion de dias");

                        Gestion_Api.Modelo.Configuracion configuracion = new Gestion_Api.Modelo.Configuracion();

                        if (!string.IsNullOrWhiteSpace(configuracion.DiasRecordatorioMail))
                        {
                            dias = Convert.ToInt32(configuracion.DiasRecordatorioMail);
                        }

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Encuentro la configuracion de dias: " + dias);

                        Mascotas_Api.Controladores.ControladorAgenda controladorAgenda = new Mascotas_Api.Controladores.ControladorAgenda();

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Busco lo agendado");


                        var agendas = controladorAgenda.ObtenerAgendasEnDias(dias);

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Encuentro agendados: " + agendas.Count);

                        if (agendas.Count > 0)
                        {
                            Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Inicio el envio de mails");

                            foreach (var agenda in agendas)
                            {
                                //int enviado = controladorAgenda.EnviarMailPacienteRecordatorio(agenda, Settings.Default.Path_Logo, Settings.Default.Link_Confirmacion + agenda.Id);

                                //if (enviado == 1)
                                //{
                                //    Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el mail");
                                //}
                                //else
                                //{
                                //    Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El mail no se envio correctamente");
                                //}

                                if (!string.IsNullOrEmpty(configuracion.EnviarSMSRecordatorio))
                                {
                                    Mascotas_Api.Controladores.ControladorPropietarios cp = new Mascotas_Api.Controladores.ControladorPropietarios();
                                    var propietarios = cp.obtenerPropietarioById(Convert.ToInt32( agenda.IdPropietario.Value));
                                    agenda.Propietarios = propietarios;

                                    Mascotas_Api.Controladores.ControladorAgenda cp2 = new Mascotas_Api.Controladores.ControladorAgenda();
                                   agenda.Agenda_Eventos = cp2.ObtenerAgendaEventos(Convert.ToInt32( agenda.Id));


                                    if (agenda.Propietarios != null && !String.IsNullOrEmpty(agenda.Propietarios.Celular))
                                    {
                                        string[] celular = agenda.Propietarios.Celular.Split('-');

                                        if (celular.Length == 2 && !String.IsNullOrEmpty(agenda.Propietarios.Celular) && celular[1].Length == 10)
                                        {
                                            string numero = celular[1];
                                            string codArea = celular[0];
                                            string mensaje = controladorAgenda.ObtenerMensajeDeTarea(agenda);

                                            string telefono = "+549" + numero;

                                            int enviadoSMS = controladorSMS.enviarSMS(telefono, mensaje, -1);
                                            if (enviadoSMS == 1)
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                            }
                                            else
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                            }
                                        }
                                        else if (agenda.Propietarios.Celular.Length == 10)
                                        {
                                            string telefono = "+549" + agenda.Propietarios.Celular;
                                            string mensaje = controladorAgenda.ObtenerMensajeDeTarea(agenda);

                                            int enviadoSMS;
                                            if (!WhatsAppHabilitado)
                                                enviadoSMS = controladorSMS.enviarSMS(telefono, mensaje, -1);
                                            else
                                                enviadoSMS = ControladorTwilioWhatsapp.EnviarWhatsaap(telefono, mensaje);

                                            if (enviadoSMS == 1)
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                            }
                                            else
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                            }

                                        }
                                        else if (agenda.Propietarios.Celular.Trim().Replace("-", "").Replace(" ", "").Length == 10)
                                        {
                                            string telefono = "+549" + agenda.Propietarios.Celular.Trim().Replace("-", "").Replace(" ", "");
                                            int enviadoSMS;
                                            string mensaje = controladorAgenda.ObtenerMensajeDeTarea(agenda);
                                            if (!WhatsAppHabilitado)
                                                enviadoSMS = controladorSMS.enviarSMS(telefono, mensaje, -1);
                                            else
                                                enviadoSMS = ControladorTwilioWhatsapp.EnviarWhatsaap(telefono, mensaje);

                                            if (enviadoSMS == 1)
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                            }
                                            else
                                            {
                                                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                            }
                                        }

                                    }
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

        public void Working3()
        {
            try
            {



                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

                Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Inicio de correr tareas");

                while (true)
                {
                    DateTime fechaActual = DateTime.Now;
                    DateTime fechaCorrer = Settings.Default.Hora_Correr;
                    Gestion_Api.Controladores.ControladorSMS controladorSMS = new Gestion_Api.Controladores.ControladorSMS();
                    ControladorTwilio ControladorTwilioWhatsapp = new ControladorTwilio();
                    bool WhatsAppHabilitado = false;
                    if (fechaActual.Hour == fechaCorrer.Hour &&
                        fechaActual.Minute == fechaCorrer.Minute)
                    {

                        int dias =3;

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Busco la configuracion de dias");

                        Gestion_Api.Modelo.Configuracion configuracion = new Gestion_Api.Modelo.Configuracion();

                        //if (!string.IsNullOrWhiteSpace(configuracion.DiasRecordatorioMail))
                        //{
                        //    dias = Convert.ToInt32(configuracion.DiasRecordatorioMail);
                        //}

                        //Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Encuentro la configuracion de dias: " + dias);

                        Mascotas_Api.Controladores.ControladorHistorial controladorHistorial = new Mascotas_Api.Controladores.ControladorHistorial();

                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Busco lo agendado");


                        var eventos = controladorHistorial.obtenerTipoEventos();
                        eventos = eventos.Where(x =>  x.Grupo == 1 && x.Vencimiento>0).ToList();


                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Eventos encontrados: " + eventos.Count);

                        if (eventos.Count > 0)
                        {
                            Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Inicio recorrido tipos de eventos");

                            foreach (var evento in eventos)
                            {

                                var HistoriasClinicas = controladorHistorial.obtenerHistoriasClinicasRecordatorio(evento, dias);

                                if (HistoriasClinicas.Count > 0)
                                {
                                    foreach (var historiaclinica in HistoriasClinicas)
                                    {
                                        if (!string.IsNullOrEmpty(configuracion.EnviarSMSRecordatorio))
                                        {


                                            Mascotas_Api.Controladores.ControladorPropietarios cp = new Mascotas_Api.Controladores.ControladorPropietarios();
                                            Mascotas_Api.Controladores.ControladorMascotas cm = new Mascotas_Api.Controladores.ControladorMascotas();
                                            var propietario = cp.ObtenerPropietarioByIdMascota(Convert.ToInt32(historiaclinica.Mascota));
                                            var mascota = cm.obtenerMascotaById(Convert.ToInt32(historiaclinica.Mascota));


                                            //Mascotas_Api.Controladores.ControladorAgenda cp2 = new Mascotas_Api.Controladores.ControladorAgenda();
                                            //agenda.Agenda_Eventos = cp2.ObtenerAgendaEventos(Convert.ToInt32(agenda.Id));


                                            if (propietario != null && !String.IsNullOrEmpty(propietario.Celular))
                                            {
                                                string[] celular = propietario.Celular.Split('-');

                                                if (celular.Length == 2 && !String.IsNullOrEmpty(propietario.Celular) && celular[1].Length == 10)
                                                {
                                                    string numero = celular[1];
                                                    string codArea = celular[0];
                                                    string mensaje = "Recordatorio:" + "el dia " + DateTime.Now.AddDays(dias).ToString("dd-MM-yyyy") + ", vencimiento " + evento.Descripcion +
                                                      " " + mascota.Nombre + " -GoPets";

                                                    string telefono = "+549" + numero;

                                                    int enviadoSMS = controladorSMS.enviarSMS(telefono, mensaje, -1);
                                                    if (enviadoSMS == 1)
                                                    {
                                                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                                    }
                                                    else
                                                    {
                                                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                                    }
                                                }
                                                else if (propietario.Celular.Length == 10)
                                                {
                                                    string telefono = "+549" + propietario.Celular;
                                                    string mensaje = "Recordatorio:" + "el dia " + DateTime.Now.AddDays(dias).ToString("dd-MM-yyyy") + ", vencimiento " + evento.Descripcion +
                                                      " " + mascota.Nombre + " -GoPets";
                                                    int enviadoSMS=0;
                                                    if (!WhatsAppHabilitado)
                                                        enviadoSMS = controladorSMS.enviarSMS(telefono, mensaje, -1);
                                                    else
                                                        enviadoSMS = ControladorTwilioWhatsapp.EnviarWhatsaap(telefono, mensaje);

                                                    if (enviadoSMS == 1)
                                                    {
                                                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                                    }
                                                    else
                                                    {
                                                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                                    }

                                                }
                                                else if (propietario.Celular.Trim().Replace("-", "").Replace(" ", "").Length == 10)
                                                {
                                                    string telefono = "+549" + propietario.Celular.Trim().Replace("-", "").Replace(" ", "");
                                                    int enviadoSMS;
                                                    string mensaje = "Recordatorio:"+ " el dia " + DateTime.Now.AddDays(dias).ToString("dd-MM-yyyy") + ", vencimiento " + evento.Descripcion +
                                                      " de - "+mascota.Nombre + " - GoPets";
                                                    if (!WhatsAppHabilitado)
                                                    {

                                                        enviadoSMS = controladorSMS.enviarSMS(telefono, mensaje, -1);
                                                        //Log.Instance.EscribirEnMsjLog(telefono +": "+mensaje);
                                                        //enviadoSMS = 1;
                                                    }
                                                    else
                                                        enviadoSMS = ControladorTwilioWhatsapp.EnviarWhatsaap(telefono, mensaje);

                                                    if (enviadoSMS == 1)
                                                    {
                                                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: Se envio correctamente el SMS");
                                                    }
                                                    else
                                                    {
                                                        Log.Instance.EscribirEnLog(DateTime.Now + " INFO: El SMS no se envio correctamente");
                                                    }
                                                }

                                            }
                                        }
                                    }

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
