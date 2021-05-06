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
        IPerfiles objPerfil;

        public RespuestaOperacionServicio BuscarPerfiles(string Sistema, string Nombre, string Estado)
        {
            List<Perfil> lstPerfiles = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPerfil = new Perfiles();
                lstPerfiles = objPerfil.buscarPerfiles(Convert.ToInt32(Sistema), Nombre, Estado);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = lstPerfiles;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ObtenerPerfil(string Id)
        {
            Perfil perfil = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPerfil = new Perfiles();
                perfil = objPerfil.obtenerPerfil(Convert.ToInt32(Id));
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = perfil;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CrearPerfil(Perfil perfil)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPerfil = new Perfiles();
                perfil = objPerfil.crearPerfil(perfil);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = perfil;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ModificarPerfil(string Id, Perfil perfil)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPerfil = new Perfiles();
                perfil.Id = Convert.ToInt32(Id);
                perfil = objPerfil.actualizarPerfil(perfil);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = perfil;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CambiarEstadoPerfil(string idUsuario, string Id, string Estado)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPerfil = new Perfiles();
                Perfil perfil = new Perfil();
                perfil.Id = Convert.ToInt32(Id);
                perfil.Estado = Convert.ToInt32(Estado);
                perfil.Usuario = Convert.ToInt32(idUsuario);
                objPerfil.cambiarEstadoPerfil(perfil);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = perfil;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CambiarEstadosPerfiles(OperacionMultiplePerfil objOperacion, string Estado, string idUsuario)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPerfil = new Perfiles();
                objPerfil.cambiarEstadosPerfiles(objOperacion.Id, Convert.ToInt32(Estado), Convert.ToInt32(idUsuario));
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
