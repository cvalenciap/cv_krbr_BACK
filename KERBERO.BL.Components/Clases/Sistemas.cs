using System;
using System.Collections.Generic;
using KERBERO.WCF.ServiceContracts;
using KERBERO.DL.Repository;
using Oracle.ManagedDataAccess.Client;
using KERBERO.Util;

namespace KERBERO.BL.Components
{
    public class Sistemas : ISistemas
    {
        private ISistemasRepositorio SistemasBD;
        private IPerfilesRepositorio PerfilesBD;
        private IPermisosRepositorio PermisosBD;
        private string strCadenaConexion;

        public Sistemas()
        {
            // obtener cadena de conexión desde el registro para SistemasBD
            strCadenaConexion = RegistroWindows.ObtenerCadenaRegistro(Configuracion.RutaKerbero, Configuracion.ClaveConnectionString);
            //strCadenaConexion = "User Id=FMV_Cerbero; Password=karma; Data Source=(DESCRIPTION =    (ADDRESS = (PROTOCOL = TCP)(HOST = 10.100.120.135)(PORT = 1523))    (CONNECT_DATA =      (SERVER = DEDICATED)      (SERVICE_NAME =DESARROLLO)    ) );";
            if (strCadenaConexion.Equals(string.Empty))
            {
                throw new Exception("No se encuentra la cadena de conexión a la base de datos");
            }
            this.SistemasBD = new SistemasRepositorio(strCadenaConexion);
            this.PerfilesBD = new PerfilesRepositorio(strCadenaConexion);
            this.PermisosBD = new PermisosRepositorio(strCadenaConexion);
        }

        public List<Sistema> buscarSistemas(string Nombre, string Estado)
        {
            List<Sistema> lstSistemas;
            lstSistemas = SistemasBD.BuscarSistemas(Nombre, Estado);
            return lstSistemas;
        }

        public Sistema obtenerSistema(int Id)
        {
            Sistema objSistema = new Sistema();
            objSistema = SistemasBD.ObtenerSistema(Id);
            return objSistema;
        }

        public Sistema crearSistema(Sistema sistema)
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
                        sistema = SistemasBD.CrearSistema(cmd, sistema);
                        int resultversion = SistemasBD.ActualizarVersionSistema(cmd, sistema.Id, sistema.Version);
                        if (sistema.Id < 0 && resultversion < 0)
                        {
                            throw new Exception("Ocurrió un error al guardar el sistema");
                        }
                        trx.Commit();
                    }
                    catch
                    {
                        trx.Rollback();
                        throw new Exception("Ocurrió un error al guardar el sistema");
                    }
                }
            }
            CerrarConexion(conn);
            return sistema;
        }

        public Sistema actualizarSistema(Sistema sistema)
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
                        int resultSistema = SistemasBD.ActualizarSistema(cmd, sistema);
                        int resultVersion = 0;
                        if (sistema.Version != String.Empty)
                        {
                            resultVersion = SistemasBD.ActualizarVersionSistema(cmd, sistema.Id, sistema.Version);
                        }
                        if (resultSistema < 0 && resultVersion < 0)
                        {
                            throw new Exception("Ocurrió un error al actualizar el sistema");
                        }
                        if (sistema.Perfiles != null)
                        {
                            for (int i = 0; i < sistema.Perfiles.Count; i++)
                            {
                                if (sistema.Perfiles[i].Id > 0/*&& sistema.Perfiles[i].Estado >= -1*/)
                                {
                                    int resultEstadoPerfil = PerfilesBD.CambiarEstadoPerfil(cmd, sistema.Perfiles[i]);
                                    if (resultEstadoPerfil < 0)
                                    {
                                        throw new Exception("Ocurrió un error al actualizar el sistema");
                                    }
                                }
                            }
                        }
                        if (sistema.Permisos != null)
                        {
                            for (int i = 0; i < sistema.Permisos.Count; i++)
                            {
                                if (sistema.Permisos[i].Id > 0 && sistema.Permisos[i].Estado >= -1)
                                {
                                    int resultEstadoPerfil = PermisosBD.CambiarEstadoPermiso(cmd, sistema.Permisos[i]);
                                    if (resultEstadoPerfil < 0)
                                    {
                                        throw new Exception("Ocurrió un error al actualizar el sistema");
                                    }
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
            return sistema;
        }

        public RespuestaOperacionServicio cambiarEstadoSistema(Sistema sistema)
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
                        int resultSistema = SistemasBD.CambiarEstadoSistema(cmd, sistema);
                        if (resultSistema < 0)
                        {
                            throw new Exception("Ocurrió un error al cambiar el estado del sistema");
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

        public RespuestaOperacionServicio cambiarEstadosSistemas(List<int> IdSistemas, int Estado, int idUsuario)
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
                        for (int i = 0; i < IdSistemas.Count; i++)
                        {
                            Sistema sistema = new Sistema();
                            sistema.Id = IdSistemas[i];
                            sistema.Estado = Estado;
                            sistema.Usuario = idUsuario;
                            int resultSistema = SistemasBD.CambiarEstadoSistema(cmd, sistema);
                            if (resultSistema < 0)
                            {
                                throw new Exception("Ocurrió un error al cambiar el estado del sistema");
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
