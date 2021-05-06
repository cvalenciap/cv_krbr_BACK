using System;
using System.Collections.Generic;
using KERBERO.WCF.ServiceContracts;
using KERBERO.BL.Components;
using KERBERO.Util;

namespace KERBERO.WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Kerbero" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Kerbero.svc o Kerbero.svc.cs en el Explorador de soluciones e inicie la depuración.
    public partial class Kerbero : IKerbero
    {
        IUsuarios objUsuario;

        public RespuestaOperacionServicio BuscarUsuarios(string ApePaterno, string ApeMaterno, string Nombre, string Login, string Estado)
        {
            List<Usuario> lstUsuarios = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objUsuario = new Usuarios();
                lstUsuarios = objUsuario.buscarUsuarios(ApePaterno, ApeMaterno, Nombre, Login, Estado);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = lstUsuarios;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ObtenerUsuario(string Id)
            {
                Usuario usuario = null;
                RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
                try
                {
                    objUsuario = new Usuarios();
                    usuario = objUsuario.obtenerUsuario(Convert.ToInt32(Id));
                    rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                    rpta.data = usuario;
                }
                catch (Exception ex)
                {
                    rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                    rpta.Error = ex.Message;
                }
                return rpta;
        }

        public RespuestaOperacionServicio CrearUsuario(Usuario usuario)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objUsuario = new Usuarios();
                usuario = objUsuario.crearUsuario(usuario);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = usuario;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ModificarUsuario(string Id, Usuario usuario)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objUsuario = new Usuarios();
                usuario.Id = Convert.ToInt32(Id);
                usuario = objUsuario.actualizarUsuario(usuario);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = usuario;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CambiarEstadoUsuario(string idUsuario, string Id, string Estado)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objUsuario = new Usuarios();
                Usuario usuario = new Usuario();
                usuario.Id = Convert.ToInt32(Id);
                usuario.Estado = Convert.ToInt32(Estado);
                usuario.User = Convert.ToInt32(idUsuario);
                objUsuario.cambiarEstadoUsuario(usuario);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = usuario;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CambiarEstadosUsuarios(OperacionMultipleUsuario objOperacion, string Estado, string idUsuario)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objUsuario = new Usuarios();
                objUsuario.cambiarEstadosUsuarios(objOperacion.Id, Convert.ToInt32(Estado), Convert.ToInt32(idUsuario));
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
