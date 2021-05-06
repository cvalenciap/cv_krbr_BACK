using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;
using KERBERO.DL.Repository;
using Oracle.ManagedDataAccess.Client;
using KERBERO.Util;

namespace KERBERO.BL.Components
{
    public class Perfiles : IPerfiles
    {
        private IPerfilesRepositorio PerfilesBD;
        private IPermisosRepositorio PermisosBD;
        private IUsuariosRepositorio UsuariosBD;
        private string strCadenaConexion;

        public Perfiles()
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

        public List<Perfil> buscarPerfiles(int IdSistema, string Nombre, string Estado)
        {
            List<Perfil> lstPerfiles = new List<Perfil>();
            lstPerfiles = PerfilesBD.BuscarPerfiles(IdSistema, Nombre, Estado);
            return lstPerfiles;
        }
        
        public Perfil obtenerPerfil(int Id)
        {
            Perfil objPerfil = new Perfil();
            objPerfil = PerfilesBD.ObtenerPerfil(Id);
            return objPerfil;
        }

        public Perfil crearPerfil(Perfil perfil)
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
                        perfil = PerfilesBD.CrearPerfil(cmd, perfil);
                        if (perfil.Id < 0)
                        {
                            throw new Exception("Ocurrió un error al guardar el perfil");
                        }
                        if (perfil.Permisos != null)
                        {
                            for (int i = 0; i < perfil.Permisos.Count; i++)
                            {
                                int resultPermXPerf = PerfilesBD.MantenimientoPermisoXPerfil(cmd, perfil.Id, perfil.Permisos[i].Id, perfil.Usuario, perfil.Permisos[i].Estado);
                                if (resultPermXPerf < 0)
                                {
                                    throw new Exception("Ocurrió un error al guardar el perfil");
                                }
                            }
                        }
                        if (perfil.Usuarios != null)
                        {
                            for (int i = 0; i < perfil.Usuarios.Count; i++)
                            {
                                int resultUserXPerm = UsuariosBD.MantenimientoUsuarioXPerfil(cmd, perfil.Usuarios[i].Id, perfil.Id, perfil.Usuario, perfil.Usuarios[i].Estado);
                                if (resultUserXPerm < 0)
                                {
                                    throw new Exception("Ocurrió un error al guardar el perfil");
                                }
                            }
                        }
                        trx.Commit();
                    }
                    catch
                    {
                        trx.Rollback();
                        throw new Exception("Ocurrió un error al guardar el perfil");
                    }
                }
            }
            CerrarConexion(conn);
            return perfil;
        }

        public Perfil actualizarPerfil(Perfil perfil)
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
                        int resultPermiso = PerfilesBD.ActualizarPerfil(cmd, perfil);
                        if (resultPermiso < 0)
                        {
                            throw new Exception("Ocurrió un error al actualizar el perfil");
                        }
                        if (perfil.Permisos != null)
                        {
                            for (int i = 0; i < perfil.Permisos.Count; i++)
                            {
                                int resultPermXPerf = PerfilesBD.MantenimientoPermisoXPerfil(cmd, perfil.Id, perfil.Permisos[i].Id, perfil.Usuario, perfil.Permisos[i].Estado);
                                if (resultPermXPerf < 0)
                                {
                                    throw new Exception("Ocurrió un error al guardar el perfil");
                                }
                            }
                        }
                        if (perfil.Usuarios != null)
                        {
                            for (int i = 0; i < perfil.Usuarios.Count; i++)
                            {
                                int resultUserXPerm = UsuariosBD.MantenimientoUsuarioXPerfil(cmd, perfil.Usuarios[i].Id, perfil.Id, perfil.Usuario, perfil.Usuarios[i].Estado);
                                if (resultUserXPerm < 0)
                                {
                                    throw new Exception("Ocurrió un error al guardar el perfil");
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
            return perfil;
        }

        public RespuestaOperacionServicio cambiarEstadoPerfil(Perfil perfil)
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
                        int resultSistema = PerfilesBD.CambiarEstadoPerfil(cmd, perfil);
                        if (resultSistema < 0)
                        {
                            throw new Exception("Ocurrió un error al cambiar el estado del perfil");
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

        public RespuestaOperacionServicio cambiarEstadosPerfiles(List<int> IdPerfiles, int Estado, int idUsuario)
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
                        for (int i = 0; i < IdPerfiles.Count; i++)
                        {
                            Perfil perfil = new Perfil();
                            perfil.Id = IdPerfiles[i];
                            perfil.Estado = Estado;
                            perfil.Usuario = idUsuario;
                            int resultSistema = PerfilesBD.CambiarEstadoPerfil(cmd, perfil);
                            if (resultSistema < 0)
                            {
                                throw new Exception("Ocurrió un error al cambiar el estado del perfil");
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
