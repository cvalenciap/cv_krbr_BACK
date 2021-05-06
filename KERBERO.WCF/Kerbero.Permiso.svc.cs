using System;
using System.Collections.Generic;
using System.Linq;
using KERBERO.BL.Components;
using KERBERO.WCF.ServiceContracts;
using KERBERO.Util;

namespace KERBERO.WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Kerbero" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Kerbero.svc o Kerbero.svc.cs en el Explorador de soluciones e inicie la depuración.
    public partial class Kerbero : IKerbero
    {
        IPermisos objPermiso;

        public RespuestaOperacionServicio BuscarPermisos(string Sistema, string Codigo, string Nombre, string Estado)
        {
            List<Permiso> lstPermisos = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPermiso = new Permisos();
                lstPermisos = objPermiso.buscarPermisos(Convert.ToInt32(Sistema), Codigo, Nombre, Estado);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = lstPermisos;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }
        
        public RespuestaOperacionServicio ObtenerPermiso(string Id)
        {
            Permiso permiso = null;
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPermiso = new Permisos();
                permiso = objPermiso.obtenerPermiso(Convert.ToInt32(Id));
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = permiso;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CrearPermiso(Permiso permiso)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPermiso = new Permisos();
                permiso = objPermiso.crearPermiso(permiso);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = permiso;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio ModificarPermiso(string Id, Permiso permiso)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPermiso = new Permisos();
                permiso.Id = Convert.ToInt32(Id);
                permiso = objPermiso.actualizarPermiso(permiso);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = permiso;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CambiarEstadoPermiso(string idUsuario, string Id, string Estado)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPermiso = new Permisos();
                Permiso permiso = new Permiso();
                permiso.Id = Convert.ToInt32(Id);
                permiso.Estado = Convert.ToInt32(Estado);
                permiso.Usuario = Convert.ToInt32(idUsuario);
                objPermiso.cambiarEstadoPermiso(permiso);
                rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                rpta.data = permiso;
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }

        public RespuestaOperacionServicio CambiarEstadosPermisos(OperacionMultiplePermiso objOperacion, string Estado, string idUsuario)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            try
            {
                objPermiso = new Permisos();
                objPermiso.cambiarEstadosPermisos(objOperacion.Id, Convert.ToInt32(Estado), Convert.ToInt32(idUsuario));
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
