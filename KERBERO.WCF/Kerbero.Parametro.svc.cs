using System;
using System.Collections.Generic;
using System.Linq;
using KERBERO.WCF.ServiceContracts;
using KERBERO.BL.Components;
using KERBERO.Util;

namespace KERBERO.WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Kerbero" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Kerbero.svc o Kerbero.svc.cs en el Explorador de soluciones e inicie la depuración.
    public partial class Kerbero : IKerbero
    {
        IParametros objParametro;

        public RespuestaOperacionServicio BuscarParametros(string ParamPadre, string Estado)
        {
            List<Parametro> lstParametros = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objParametro = new Parametros();
                lstParametros = objParametro.buscarParametros(ParamPadre, Estado);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = lstParametros;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ObtenerParametro(string Id)
        {
            Parametro parametro = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objParametro = new Parametros();
                parametro = objParametro.obtenerParametro(Convert.ToInt32(Id));
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = parametro;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ValorParametro(string Id)
        {
            Parametro parametro = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objParametro = new Parametros();
                parametro = objParametro.valorParametro(Convert.ToInt32(Id));
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = parametro;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CrearParametro(Parametro parametro)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objParametro = new Parametros();
                parametro = objParametro.crearParametro(parametro);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = parametro;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ModificarParametro(string Id, Parametro parametro)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objParametro = new Parametros();
                parametro.Id = Convert.ToInt32(Id);
                parametro = objParametro.actualizarParametro(parametro);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = parametro;
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
