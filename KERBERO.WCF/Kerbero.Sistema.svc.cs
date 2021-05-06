using System;
using System.Collections.Generic;
using System.Linq;
using KERBERO.WCF.ServiceContracts;
using KERBERO.BL.Components;
using KERBERO.Util;
using Oracle.ManagedDataAccess.Client;

namespace KERBERO.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Kerbero : IKerbero
    {
        ISistemas objSistema;

        public RespuestaOperacionServicio BuscarSistemas(string Nombre, string Estado)
        {
            List<Sistema> lstSistemas = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objSistema = new Sistemas();
                lstSistemas = objSistema.buscarSistemas(Nombre, Estado);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = lstSistemas;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ObtenerSistema(string Id)
        {
            Sistema sistema = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objSistema = new Sistemas();
                sistema = objSistema.obtenerSistema(Convert.ToInt32(Id));
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = sistema;
            }
            catch (Exception ex)
            {
                RegistroEventos.RegistrarError(ex);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CrearSistema(Sistema sistema)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objSistema = new Sistemas();
                sistema = objSistema.crearSistema(sistema);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = sistema;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ModificarSistema(string Id, Sistema sistema)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objSistema = new Sistemas();
                sistema.Id = Convert.ToInt32(Id);
                sistema = objSistema.actualizarSistema(sistema);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = sistema;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CambiarEstadoSistema(string idUsuario, string Id, string Estado)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objSistema = new Sistemas();
                Sistema sistema = new Sistema();
                sistema.Id = Convert.ToInt32(Id);
                sistema.Estado = Convert.ToInt32(Estado);
                sistema.Usuario = Convert.ToInt32(idUsuario);
                objSistema.cambiarEstadoSistema(sistema);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = sistema;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CambiarEstadosSistemas(OperacionMultipleSistema objOperacion, string Estado, string idUsuario)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objSistema = new Sistemas();
                objSistema.cambiarEstadosSistemas(objOperacion.Id, Convert.ToInt32(Estado), Convert.ToInt32(idUsuario));
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }
    }
}
