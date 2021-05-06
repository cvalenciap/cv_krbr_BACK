///-------------------------------------------------------------------------------------
///   Namespace:        CERBERO.Util
///   Objeto:           RegistroEventos
///   Descripcion:      Métodos para gestionar el registro de eventos de Windows (EventLog)
///   Autor:            Daniel Salas
///-------------------------------------------------------------------------------------
///   Historia de modificaciones:
///   Requerimiento:    Autor:            Fecha:        Descripcion:
///-------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KERBERO.Util
{
    /// <summary>
    /// Métodos para gestionar el registro de eventos de Windows (EventLog)
    /// </summary>
    public static class RegistroEventos
    {

        private const string LOG_NAME = "Kerbero";
        public const string FUENTE_WEB_API = "KERBERO.WCF";
        //public const string FUENTE_REPO_BD = "Kerbero BD";

        /// <summary>
        /// Registrar un mensaje en el registro de eventos
        /// </summary>
        /// <param name="strMensaje">texto del mensaje</param>
        /// <param name="strFuente">nombre de la fuente</param>
        /// <param name="intTipo">tipo de evento</param>
        public static void RegistrarMensaje(string strMensaje, string strFuente = FUENTE_WEB_API, int intTipo = (int)EventLogEntryType.Warning)
        {
            Registrar(strFuente, (EventLogEntryType)intTipo, strMensaje);
        }

        /// <summary>
        /// Registrar una excepción en el registro de eventos
        /// </summary>
        /// <param name="exError">excepción</param>
        /// <param name="strFuente">nombre de la fuente</param>
        public static void RegistrarError(Exception exError, string strFuente = FUENTE_WEB_API)
        {
            try
            {
                Registrar(strFuente, EventLogEntryType.Error, string.Format(
                    "Message: {0}\r\n\r\nSource: {1}\r\n\r\nTarget site: {2}\r\n\r\nStack Trace:\r\n{3}",
                     exError.Message, exError.Source.ToString(), exError.TargetSite.ToString(), exError.StackTrace)
                );
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        private static void Registrar(string strFuente, EventLogEntryType evTipo, string strMensaje)
        {
            int Id = 0;

            EventLog evLog = new EventLog(LOG_NAME);
            evLog.Source = strFuente;
            evLog.WriteEntry(strMensaje, evTipo, Id);
        }

        
    }
}
