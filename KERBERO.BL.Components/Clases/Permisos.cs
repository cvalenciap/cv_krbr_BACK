using System;
using System.Collections.Generic;
using KERBERO.WCF.ServiceContracts;
using KERBERO.DL.Repository;
using Oracle.ManagedDataAccess.Client;
using KERBERO.Util;

namespace KERBERO.BL.Components
{
    public class Permisos : IPermisos
    {        
        private IPermisosRepositorio PermisosBD;
        private IPerfilesRepositorio PerfilesBD;
        private IUsuariosRepositorio UsuariosBD;
        private string strCadenaConexion;

        public Permisos()
        {
            // obtener cadena de conexión desde el registro para SistemasBD
            strCadenaConexion = RegistroWindows.ObtenerCadenaRegistro(Configuracion.RutaKerbero, Configuracion.ClaveConnectionString);
            //strCadenaConexion = "User Id=FMV_Cerbero; Password=karma; Data Source=(DESCRIPTION =    (ADDRESS = (PROTOCOL = TCP)(HOST = 10.100.120.135)(PORT = 1523))    (CONNECT_DATA =      (SERVER = DEDICATED)      (SERVICE_NAME =DESARROLLO)    ) );";
            if (strCadenaConexion.Equals(string.Empty))
            {
                throw new Exception("No se encuentra la cadena de conexión a la base de datos");
            }
            this.PerfilesBD = new PerfilesRepositorio(strCadenaConexion);
            this.PermisosBD = new PermisosRepositorio(strCadenaConexion);
            this.UsuariosBD = new UsuariosRepositorio(strCadenaConexion);
        }

        public List<Permiso> buscarPermisos(int IdSistema, string Codigo, string Nombre, string Estado)
        {
            List<Permiso> lstPermisos;
            lstPermisos = PermisosBD.BuscarPermisos(IdSistema, Codigo, Nombre, Estado);
            return lstPermisos;
        }

        public Permiso obtenerPermiso(int Id)
        {
            Permiso objPermiso = new Permiso();
            objPermiso = PermisosBD.ObtenerPermiso(Id);
            return objPermiso;
        }

        public Permiso crearPermiso(Permiso permiso)
        {
            OracleConnection conn = null;
            try
            {
                conn = new OracleConnection(this.strCadenaConexion);
                conn.Open();
            }
            catch
            {
                throw new Exception("Error de conexión a la Base de Datos");
            }
            using (OracleCommand cmd = conn.CreateCommand())
            {
                using (OracleTransaction trx = conn.BeginTransaction())
                {
                    cmd.Transaction = trx;
                    try
                    {
                        permiso = PermisosBD.CrearPermiso(cmd, permiso);
                        if (permiso.Id < 0)
                        {
                            throw new Exception("Ocurrió un error al guardar el permiso");
                        }
                        if (permiso.Perfiles != null)
                        {
                            for (int i = 0; i < permiso.Perfiles.Count; i++)
                            {
                                int resultPermXPerf = PerfilesBD.MantenimientoPermisoXPerfil(cmd, permiso.Perfiles[i].Id, permiso.Id, permiso.Usuario, permiso.Perfiles[i].Estado);
                                if (resultPermXPerf < 0)
                                {
                                    throw new Exception("Ocurrió un error al guardar el permiso");
                                }
                            }
                        }
                        if (permiso.Usuarios != null)
                        {
                            for (int i = 0; i < permiso.Usuarios.Count; i++)
                            {
                                int resultUserXPerm = UsuariosBD.MantenimientoUsuarioXPermiso(cmd, permiso.Usuarios[i].Id, permiso.Id, permiso.Usuario, permiso.Usuarios[i].Estado);
                                if (resultUserXPerm < 0)
                                {
                                    throw new Exception("Ocurrió un error al guardar el permiso");
                                }
                            }
                        }
                        trx.Commit();
                    }
                    catch
                    {
                        trx.Rollback();
                        throw new Exception("Ocurrió un error al guardar el permiso");
                    }
                }
            }
            CerrarConexion(conn);
            return permiso;
        }

        public Permiso actualizarPermiso(Permiso permiso)
        {
            OracleConnection conn = null;
            try
            {
                conn = new OracleConnection(this.strCadenaConexion);
                conn.Open();
            }
            catch
            {
                throw new Exception("Error de conexión a la Base de Datos");
            }
            using (OracleCommand cmd = conn.CreateCommand())
            {
                using (OracleTransaction trx = conn.BeginTransaction())
                {
                    cmd.Transaction = trx;
                    try
                    {
                        int resultPermiso = PermisosBD.ActualizarPermiso(cmd, permiso);
                        if (resultPermiso < 0)
                        {
                            throw new Exception("Ocurrió un error al actualizar el permiso");
                        }
                        if (permiso.Perfiles != null)
                        {
                            for (int i = 0; i < permiso.Perfiles.Count; i++)
                            {
                                int resultPermXPerf = PerfilesBD.MantenimientoPermisoXPerfil(cmd, permiso.Perfiles[i].Id, permiso.Id, permiso.Usuario, permiso.Perfiles[i].Estado);
                                if (resultPermXPerf < 0)
                                {
                                    throw new Exception("Ocurrió un error al actualizar el permiso");
                                }
                            }
                        }
                        if (permiso.Usuarios != null)
                        {
                            for (int i = 0; i < permiso.Usuarios.Count; i++)
                            {
                                int resultUserXPerm = UsuariosBD.MantenimientoUsuarioXPermiso(cmd, permiso.Usuarios[i].Id, permiso.Id, permiso.Usuario, permiso.Usuarios[i].Estado);
                                if (resultUserXPerm < 0)
                                {
                                    throw new Exception("Ocurrió un error al actualizar el permiso");
                                }
                            }
                        }
                        trx.Commit();
                    }
                    catch (Exception ex)
                    {
                        trx.Rollback();
                        throw ex;
                    }
                }
            }
            CerrarConexion(conn);
            return permiso;
        }

        public RespuestaOperacionServicio cambiarEstadoPermiso(Permiso permiso)
        {
            RespuestaOperacionServicio objRespuesta = new RespuestaOperacionServicio();
            OracleConnection conn = null;
            try
            {
                conn = new OracleConnection(this.strCadenaConexion);
                conn.Open();
            }
            catch
            {
                throw new Exception("Error de conexión a la Base de Datos");
            }
            using (OracleCommand cmd = conn.CreateCommand())
            {
                using (OracleTransaction trx = conn.BeginTransaction())
                {
                    cmd.Transaction = trx;
                    try
                    {
                        int resultSistema = PermisosBD.CambiarEstadoPermiso(cmd, permiso);
                        if (resultSistema < 0)
                        {
                            throw new Exception("Ocurrió un error al cambiar el estado del permiso");
                        }
                        trx.Commit();
                        objRespuesta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                    }
                    catch (Exception ex)
                    {
                        trx.Rollback();
                        objRespuesta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                        objRespuesta.Error = ex.Message;
                    }
                }
            }
            CerrarConexion(conn);
            return objRespuesta;
        }

        public RespuestaOperacionServicio cambiarEstadosPermisos(List<int> IdPermisos, int Estado, int idUsuario)
        {
            RespuestaOperacionServicio objRespuesta = new RespuestaOperacionServicio();
            OracleConnection conn = null;
            try
            {
                conn = new OracleConnection(this.strCadenaConexion);
                conn.Open();
            }
            catch
            {
                throw new Exception("Error de conexión a la Base de Datos");
            }
            using (OracleCommand cmd = conn.CreateCommand())
            {
                using (OracleTransaction trx = conn.BeginTransaction())
                {
                    cmd.Transaction = trx;
                    try
                    {
                        for (int i = 0; i < IdPermisos.Count; i++)
                        {
                            Permiso permiso = new Permiso();
                            permiso.Id = IdPermisos[i];
                            permiso.Estado = Estado;
                            permiso.Usuario = idUsuario;
                            int resultSistema = PermisosBD.CambiarEstadoPermiso(cmd, permiso);
                            if (resultSistema < 0)
                            {
                                throw new Exception("Ocurrió un error al cambiar el estado del permiso");
                            }
                        }
                        trx.Commit();
                        objRespuesta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                    }
                    catch (Exception ex)
                    {
                        trx.Rollback();
                        objRespuesta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                        objRespuesta.Error = ex.Message;
                    }
                }
            }
            CerrarConexion(conn);
            return objRespuesta;
        }

        private void CerrarConexion(OracleConnection conn)
        {
            if (conn != null)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }
    }
}
