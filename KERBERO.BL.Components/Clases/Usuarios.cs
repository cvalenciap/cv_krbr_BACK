using System;
using System.Collections.Generic;
using KERBERO.WCF.ServiceContracts;
using KERBERO.DL.Repository;
using Oracle.ManagedDataAccess.Client;
using KERBERO.Util;

namespace KERBERO.BL.Components
{
    public class Usuarios : IUsuarios
    {
        private IUsuariosRepositorio UsuariosBD;
        private string strCadenaConexion;

        public Usuarios()
        {
            // obtener cadena de conexión desde el registro para SistemasBD
            strCadenaConexion = RegistroWindows.ObtenerCadenaRegistro(Configuracion.RutaKerbero, Configuracion.ClaveConnectionString);
            //strCadenaConexion = "User Id=FMV_Cerbero; Password=karma; Data Source=(DESCRIPTION =    (ADDRESS = (PROTOCOL = TCP)(HOST = 10.100.120.135)(PORT = 1523))    (CONNECT_DATA =      (SERVER = DEDICATED)      (SERVICE_NAME =DESARROLLO)    ) );";
            if (strCadenaConexion.Equals(string.Empty))
            {
                throw new Exception("No se encuentra la cadena de conexión a la base de datos");
            }
            this.UsuariosBD = new UsuariosRepositorio(strCadenaConexion);
        }

        public List<Usuario> buscarUsuarios(string ApePaterno, string ApeMaterno, string Nombre, string Login, string Estado)
        {
            List<Usuario> lstUsuarios = new List<Usuario>();
            lstUsuarios = UsuariosBD.BuscarUsuarios(ApePaterno, ApeMaterno, Nombre, Login, Estado);
            return lstUsuarios;
        }

        public Usuario obtenerUsuario(int Id)
        {
            Usuario objUsuario = new Usuario();
            objUsuario = UsuariosBD.ObtenerUsuario(Id);
            return objUsuario;
        }

        public Usuario crearUsuario(Usuario usuario)
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
                        usuario = UsuariosBD.CrearUsuario(cmd, usuario);
                        usuario.Contrasena = Encriptacion.MD5(usuario.Contrasena);
                        int contrasenaResul = UsuariosBD.ActualizarContrasena(cmd, usuario.Id, usuario.Contrasena, usuario.User);
                        if (usuario.Id < 0 && contrasenaResul < 0)
                        {
                            throw new Exception("Ocurrió un error al guardar el usuario");
                        }
                        if (usuario.Permisos != null)
                        {
                            for (int i = 0; i < usuario.Permisos.Count; i++)
                            {
                                int resultPermXUsr = UsuariosBD.MantenimientoUsuarioXPermiso(cmd, usuario.Id, usuario.Permisos[i].Id, usuario.User, usuario.Permisos[i].Estado);
                                if (resultPermXUsr < 0)
                                {
                                    throw new Exception("Ocurrió un error al guardar el usuario");
                                }
                            }
                        }
                        if (usuario.Perfiles != null)
                        {
                            for (int i = 0; i < usuario.Perfiles.Count; i++)
                            {
                                int resultUserXPerf = UsuariosBD.MantenimientoUsuarioXPerfil(cmd, usuario.Id, usuario.Perfiles[i].Id, usuario.User, usuario.Perfiles[i].Estado);
                                if (resultUserXPerf < 0)
                                {
                                    throw new Exception("Ocurrió un error al guardar el usuario");
                                }
                            }
                        }
                        trx.Commit();
                    }
                    catch
                    {
                        trx.Rollback();
                        throw new Exception("Ocurrió un error al guardar el usuario");
                    }
                }
            }
            CerrarConexion(conn);
            return usuario;
        }

        public Usuario actualizarUsuario(Usuario usuario)
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
                        int resultUsuario = UsuariosBD.ActualizarUsuario(cmd, usuario);
                        int contrasenaResul = 0;
                        if (usuario.Contrasena != String.Empty)
                        {
                            usuario.Contrasena = Encriptacion.MD5(usuario.Contrasena);
                            contrasenaResul = UsuariosBD.ActualizarContrasena(cmd, usuario.Id, usuario.Contrasena, usuario.User);
                        }
                        if (resultUsuario < 0 && contrasenaResul < 0)
                        {
                            throw new Exception("Ocurrió un error al actualizar el usuario");
                        }
                        if (usuario.Permisos != null)
                        {
                            for (int i = 0; i < usuario.Permisos.Count; i++)
                            {
                                int resultPermXUsr = UsuariosBD.MantenimientoUsuarioXPermiso(cmd, usuario.Id, usuario.Permisos[i].Id, usuario.User, usuario.Permisos[i].Estado);
                                if (resultPermXUsr < 0)
                                {
                                    throw new Exception("Ocurrió un error al actualizar el usuario");
                                }
                            }
                        }
                        if (usuario.Perfiles != null)
                        {
                            for (int i = 0; i < usuario.Perfiles.Count; i++)
                            {
                                int resultUserXPerf = UsuariosBD.MantenimientoUsuarioXPerfil(cmd, usuario.Id, usuario.Perfiles[i].Id, usuario.User, usuario.Perfiles[i].Estado);
                                if (resultUserXPerf < 0)
                                {
                                    throw new Exception("Ocurrió un error al actualizar el usuario");
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
            return usuario;
        }

        public RespuestaOperacionServicio cambiarEstadoUsuario(Usuario usuario)
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
                        int resultUsuario = UsuariosBD.CambiarEstadoUsuario(cmd, usuario);
                        if (resultUsuario < 0)
                        {
                            throw new Exception("Ocurrió un error al cambiar el estado del usuario");
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

        public RespuestaOperacionServicio cambiarEstadosUsuarios(List<int> IdUsuarios, int Estado, int idUsuario)
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
                        for (int i = 0; i < IdUsuarios.Count; i++)
                        {
                            Usuario usuario = new Usuario();
                            usuario.Id = IdUsuarios[i];
                            usuario.Estado = Estado;
                            usuario.User = idUsuario;
                            int resultUsuario = UsuariosBD.CambiarEstadoUsuario(cmd, usuario);
                            if (resultUsuario < 0)
                            {
                                throw new Exception("Ocurrió un error al cambiar el estado del usuario");
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

        public string obtenerIdUsuario(string Login)
        {
            return UsuariosBD.ObtenerIdUsuario(Login);
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
