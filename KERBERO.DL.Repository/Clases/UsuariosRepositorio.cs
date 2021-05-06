using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace KERBERO.DL.Repository
{
    public class UsuariosRepositorio : IUsuariosRepositorio
    {
        private OracleConnection conn;
        private string Package;
        public const int RESULTADO_OK = 1;
        public const int RESULTADO_ERROR = -1;
        public string cadenaConexion;

        public UsuariosRepositorio(string strCadenaConexion)
        {
            cadenaConexion = strCadenaConexion;
            Package = "PKGSEG_KERBERO";
            //conn = new OracleConnection(strCadenaConexion);
            try
            {
                //conn.Open();
            }
            catch (OracleException ex)
            {
                //RegistroEventos.RegistrarMensaje("Error de conexión a base de datos:\n\r" + conn.ConnectionString, RegistroEventos.FUENTE_REPO_BD);
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarMensaje("Error de conexión a base de datos:\n\r" + conn.ConnectionString, RegistroEventos.FUENTE_REPO_BD);
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
            }
        }

        public List<Usuario> BuscarUsuarios(string ApePaterno, string ApeMaterno, string Nombre, string Login, string Estado)
        {
            try
            {
                conn = new OracleConnection(cadenaConexion);
                conn.Open();
            }
            catch
            {
                throw new Exception("Error de conexión a la Base de Datos");
            }
            try
            {
                //conn = new OracleConnection(cadenaConexion);
                //conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = Package + ".SPRKERB_BUSCAR_USUARIOS";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter isApePaterno = new OracleParameter("ISApePat", OracleDbType.Varchar2);
                isApePaterno.Value = ApePaterno;
                cmd.Parameters.Add(isApePaterno);

                OracleParameter isApeMaterno = new OracleParameter("ISApeMat", OracleDbType.Varchar2);
                isApeMaterno.Value = ApeMaterno;
                cmd.Parameters.Add(isApeMaterno);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isLogin = new OracleParameter("ISLogin", OracleDbType.Varchar2);
                isLogin.Value = Login;
                cmd.Parameters.Add(isLogin);

                OracleParameter isEstado = new OracleParameter("ISEstado", OracleDbType.Varchar2);
                isEstado.Value = Estado;
                cmd.Parameters.Add(isEstado);

                OracleParameter ocUsuarios = new OracleParameter();
                ocUsuarios.OracleDbType = OracleDbType.RefCursor;
                ocUsuarios.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocUsuarios);

                OracleDataReader reader = cmd.ExecuteReader();
                List<Usuario> ListaUsuarios = new List<Usuario>();
                Usuario usuario;

                while (reader.Read())
                {
                    usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["CUSR_ID"].ToString());
                    usuario.Login = reader["SUSR_LOGIN"].ToString();
                    string apePaterno = reader["SUSR_APELLIDO_PATERNO"].ToString();
                    string apeMaterno = reader["SUSR_APELLIDO_MATERNO"].ToString();
                    string nombre = reader["SUSR_NOMBRE"].ToString();
                    string nombreCompleto = nombre + " " + apePaterno + " " + apeMaterno;
                    usuario.NombreCompleto = nombreCompleto.Trim();
                    usuario.FechaCaducidad = Convert.ToDateTime(reader["DUSR_FECHA_CADUCIDAD"].ToString()).ToShortDateString();
                    usuario.Estado = Convert.ToInt32(reader["FUSR_ESTADO"].ToString());
                    usuario.FechaRegistro = Convert.ToDateTime(reader["DAUD_CREAR_FECHA"].ToString()).ToShortDateString();
                    ListaUsuarios.Add(usuario);
                }
                CerrarConexion(cmd);
                return ListaUsuarios;
            }
            catch 
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw new Exception("Error al buscar usuarios");
            }
        }

        public Usuario ObtenerUsuario(int Id)
        {
            try
            {
                conn = new OracleConnection(cadenaConexion);
                conn.Open();
            }
            catch
            {
                throw new Exception("Error de conexión a la Base de Datos");
            }
            try
            {
                //conn = new OracleConnection(cadenaConexion);
                //conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = Package + ".SPRKERB_OBTENER_USUARIO";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Varchar2);
                inID.Value = Id;
                cmd.Parameters.Add(inID);

                OracleParameter ocUsuario = new OracleParameter();
                ocUsuario.OracleDbType = OracleDbType.RefCursor;
                ocUsuario.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocUsuario);

                OracleParameter ocPermisos = new OracleParameter();
                ocPermisos.OracleDbType = OracleDbType.RefCursor;
                ocPermisos.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPermisos);

                OracleParameter ocPerfiles = new OracleParameter();
                ocPerfiles.OracleDbType = OracleDbType.RefCursor;
                ocPerfiles.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPerfiles);

                OracleDataReader reader = cmd.ExecuteReader();
                Usuario usuario = new Usuario();

                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["CUSR_ID"].ToString());
                    usuario.ApePaterno = reader["SUSR_APELLIDO_PATERNO"].ToString();
                    usuario.ApeMaterno = reader["SUSR_APELLIDO_MATERNO"].ToString();
                    usuario.Nombre = reader["SUSR_NOMBRE"].ToString();
                    usuario.Login = reader["SUSR_LOGIN"].ToString();
                    usuario.Email = reader["SUSR_EMAIL"].ToString();
                    usuario.FechaCaducidad = Convert.ToDateTime(reader["DUSR_FECHA_CADUCIDAD"].ToString()).ToShortDateString();
                    usuario.Estado = Convert.ToInt32(reader["FUSR_ESTADO"].ToString());
                    usuario.FechaRegistro = Convert.ToDateTime(reader["DAUD_CREAR_FECHA"].ToString()).ToShortDateString();
                    usuario.HorarioAcceso = reader["SUSR_ACCESOHORARIO"].ToString();
                    usuario.Permisos = new List<Permiso>();
                    usuario.Perfiles = new List<Perfil>();
                }

                //Permisos del Usuario
                reader.NextResult();
                while (reader.Read())
                {
                    Permiso permisoUser = new Permiso();
                    permisoUser.Id = Convert.ToInt32(reader["CPRM_ID"].ToString());
                    permisoUser.Sistema = new Sistema();
                    permisoUser.Sistema.Id = Convert.ToInt32(reader["CSIS_ID"].ToString());
                    permisoUser.Sistema.Nombre = reader["SSIS_NOMBRE"].ToString();
                    permisoUser.Estado = Convert.ToInt32(reader["FPRM_ESTADO"].ToString());
                    permisoUser.Codigo = reader["SPRM_CODIGO"].ToString();
                    permisoUser.Descripcion = reader["SPRM_DESCRIPCION"].ToString();
                    usuario.Permisos.Add(permisoUser);
                }

                //Perfiles del Usuario
                reader.NextResult();
                while (reader.Read())
                {
                    Perfil PerfilUser = new Perfil();
                    PerfilUser.Id = Convert.ToInt32(reader["CPFL_ID"].ToString());
                    PerfilUser.Sistema = new Sistema();
                    PerfilUser.Sistema.Id = Convert.ToInt32(reader["CSIS_ID"].ToString());
                    PerfilUser.Sistema.Nombre = reader["SSIS_NOMBRE"].ToString();
                    PerfilUser.Estado = Convert.ToInt32(reader["FPFL_ESTADO"].ToString());
                    PerfilUser.Codigo = reader["SPFL_CODIGO"].ToString();
                    PerfilUser.Nombre = reader["SPFL_DESCRIPCION"].ToString();
                    usuario.Perfiles.Add(PerfilUser);
                }

                CerrarConexion(cmd);
                return usuario;
            }
            catch
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw new Exception("Error al obtener usuario");
            }
        }

        public Usuario CrearUsuario(OracleCommand cmd, Usuario ObjUsuario)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_INSERTAR_USUARIO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter isLogin = new OracleParameter("ISLogin", OracleDbType.Varchar2);
                isLogin.Value = ObjUsuario.Login;
                cmd.Parameters.Add(isLogin);

                OracleParameter isApePaterno = new OracleParameter("ISApePat", OracleDbType.Varchar2);
                isApePaterno.Value = ObjUsuario.ApePaterno;
                cmd.Parameters.Add(isApePaterno);

                OracleParameter isApeMaterno = new OracleParameter("ISApeMat", OracleDbType.Varchar2);
                isApeMaterno.Value = ObjUsuario.ApeMaterno;
                cmd.Parameters.Add(isApeMaterno);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = ObjUsuario.Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isEmail = new OracleParameter("ISEmail", OracleDbType.Varchar2);
                isEmail.Value = ObjUsuario.Email;
                cmd.Parameters.Add(isEmail);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjUsuario.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inNroIntento = new OracleParameter("IFNrIntento", OracleDbType.Int32);
                inNroIntento.Value = ObjUsuario.NroIntento;
                cmd.Parameters.Add(inNroIntento);

                OracleParameter idFecCaducidad = new OracleParameter("ISFecCaduc", OracleDbType.Varchar2);
                idFecCaducidad.Value = ObjUsuario.FechaCaducidad;
                cmd.Parameters.Add(idFecCaducidad);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjUsuario.User;
                cmd.Parameters.Add(inUsuario);

                OracleParameter isHorario = new OracleParameter("ISHorario", OracleDbType.Varchar2);
                isHorario.Value = ObjUsuario.HorarioAcceso;
                cmd.Parameters.Add(isHorario);

                OracleParameter inReqValidacion = new OracleParameter("IFReqValid", OracleDbType.Int32);
                inReqValidacion.Value = ObjUsuario.ReqValidacion;
                cmd.Parameters.Add(inReqValidacion);

                OracleParameter isLlavePC = new OracleParameter("ISLlavePC", OracleDbType.Varchar2);
                isLlavePC.Value = ObjUsuario.LlavePC;
                cmd.Parameters.Add(isLlavePC);

                OracleParameter inPermisoLlave = new OracleParameter("IFPermLlave", OracleDbType.Int32);
                inPermisoLlave.Value = ObjUsuario.PermisoLlavePC;
                cmd.Parameters.Add(inPermisoLlave);

                //OracleParameter ocEstadoOper = new OracleParameter("iintEstadoOper", OracleDbType.Int32);
                //ocEstadoOper.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(ocEstadoOper);

                OracleParameter ocIDGenerado = new OracleParameter("OFId", OracleDbType.Int32);
                ocIDGenerado.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocIDGenerado);

                cmd.ExecuteNonQuery();

                //int s_id_resultado = Convert.ToInt32(cmd.Parameters["iintEstadoOper"].Value.ToString());
                ObjUsuario.Id = Convert.ToInt32(cmd.Parameters["OFId"].Value.ToString());
                //CerrarConexion(cmd);

                return ObjUsuario;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int ActualizarUsuario(OracleCommand cmd, Usuario ObjUsuario)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_ACTUALIZAR_USUARIO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjUsuario.Id;
                cmd.Parameters.Add(inID);

                OracleParameter isLogin = new OracleParameter("ISLogin", OracleDbType.Varchar2);
                isLogin.Value = ObjUsuario.Login;
                cmd.Parameters.Add(isLogin);

                OracleParameter isApePaterno = new OracleParameter("ISApePat", OracleDbType.Varchar2);
                isApePaterno.Value = ObjUsuario.ApePaterno;
                cmd.Parameters.Add(isApePaterno);

                OracleParameter isApeMaterno = new OracleParameter("ISApeMat", OracleDbType.Varchar2);
                isApeMaterno.Value = ObjUsuario.ApeMaterno;
                cmd.Parameters.Add(isApeMaterno);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = ObjUsuario.Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isEmail = new OracleParameter("ISEmail", OracleDbType.Varchar2);
                isEmail.Value = ObjUsuario.Email;
                cmd.Parameters.Add(isEmail);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjUsuario.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inNroIntento = new OracleParameter("IFNrIntento", OracleDbType.Int32);
                inNroIntento.Value = ObjUsuario.NroIntento;
                cmd.Parameters.Add(inNroIntento);

                OracleParameter idFecCaducidad = new OracleParameter("ISFecCaduc", OracleDbType.Varchar2);
                idFecCaducidad.Value = ObjUsuario.FechaCaducidad;
                cmd.Parameters.Add(idFecCaducidad);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjUsuario.User;
                cmd.Parameters.Add(inUsuario);

                OracleParameter isHorario = new OracleParameter("ISHorario", OracleDbType.Varchar2);
                isHorario.Value = ObjUsuario.HorarioAcceso;
                cmd.Parameters.Add(isHorario);

                OracleParameter inReqValidacion = new OracleParameter("IFReqValid", OracleDbType.Int32);
                inReqValidacion.Value = ObjUsuario.ReqValidacion;
                cmd.Parameters.Add(inReqValidacion);

                OracleParameter isLlavePC = new OracleParameter("ISLlavePC", OracleDbType.Varchar2);
                isLlavePC.Value = ObjUsuario.LlavePC;
                cmd.Parameters.Add(isLlavePC);

                OracleParameter inPermisoLlave = new OracleParameter("IFPermLlave", OracleDbType.Int32);
                inPermisoLlave.Value = ObjUsuario.PermisoLlavePC;
                cmd.Parameters.Add(inPermisoLlave);

                OracleParameter ocEstadoOper = new OracleParameter("OFEstadoOper", OracleDbType.Int32);
                ocEstadoOper.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocEstadoOper);

                cmd.ExecuteNonQuery();

                int s_id_resultado;
                s_id_resultado = Convert.ToInt32(cmd.Parameters["OFEstadoOper"].Value.ToString());
                //CerrarConexion(cmd);

                return s_id_resultado;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int CambiarEstadoUsuario(OracleCommand cmd, Usuario ObjUsuario)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_CAMBIAR_ESTADO_USUARIO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjUsuario.Id;
                cmd.Parameters.Add(inID);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjUsuario.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjUsuario.User;
                cmd.Parameters.Add(inUsuario);

                OracleParameter ocEstadoOper = new OracleParameter("OFEstadoOper", OracleDbType.Int32);
                ocEstadoOper.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocEstadoOper);

                cmd.ExecuteNonQuery();

                int s_id_resultado;
                s_id_resultado = Convert.ToInt32(cmd.Parameters["OFEstadoOper"].Value.ToString());
                //CerrarConexion(cmd);

                return s_id_resultado;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int MantenimientoUsuarioXPermiso(OracleCommand cmd, int IDUsuario, int IDPermiso, int Usuario, int Estado)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_MANT_USRXPERM";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inIDPermiso = new OracleParameter("IFIdPerm", OracleDbType.Int32);
                inIDPermiso.Value = IDPermiso;
                cmd.Parameters.Add(inIDPermiso);

                OracleParameter inIDUsuario = new OracleParameter("IFIdUsuario", OracleDbType.Int32);
                inIDUsuario.Value = IDUsuario;
                cmd.Parameters.Add(inIDUsuario);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = Usuario;
                cmd.Parameters.Add(inUsuario);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter ocEstadoOper = new OracleParameter("OFEstadoOper", OracleDbType.Int32);
                ocEstadoOper.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocEstadoOper);

                cmd.ExecuteNonQuery();

                int s_id_resultado;
                s_id_resultado = Convert.ToInt32(cmd.Parameters["OFEstadoOper"].Value.ToString());
                //CerrarConexion(cmd);

                return s_id_resultado;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int MantenimientoUsuarioXPerfil(OracleCommand cmd, int IDUsuario, int IDPerfil, int Usuario, int Estado)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_MANT_USRXPERF";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inIDPerfil = new OracleParameter("IFIdPerf", OracleDbType.Int32);
                inIDPerfil.Value = IDPerfil;
                cmd.Parameters.Add(inIDPerfil);

                OracleParameter inIDUsuario = new OracleParameter("IFIdUsuario", OracleDbType.Int32);
                inIDUsuario.Value = IDUsuario;
                cmd.Parameters.Add(inIDUsuario);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = Usuario;
                cmd.Parameters.Add(inUsuario);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter ocEstadoOper = new OracleParameter("OFEstadoOper", OracleDbType.Int32);
                ocEstadoOper.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocEstadoOper);

                cmd.ExecuteNonQuery();

                int s_id_resultado;
                s_id_resultado = Convert.ToInt32(cmd.Parameters["OFEstadoOper"].Value.ToString());
                //CerrarConexion(cmd);

                return s_id_resultado;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int ActualizarContrasena(OracleCommand cmd, int IDUsuario, string Contrasena, int Usuario)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_ACTUALIZAR_CONTRASENA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inIDUser = new OracleParameter("IFIdUser", OracleDbType.Int32);
                inIDUser.Value = IDUsuario;
                cmd.Parameters.Add(inIDUser);

                OracleParameter isContrasena = new OracleParameter("ISContasena", OracleDbType.Varchar2);
                isContrasena.Value = Contrasena;
                cmd.Parameters.Add(isContrasena);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = Usuario;
                cmd.Parameters.Add(inUsuario);

                OracleParameter ocEstadoOper = new OracleParameter("OFEstadoOper", OracleDbType.Int32);
                ocEstadoOper.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocEstadoOper);

                cmd.ExecuteNonQuery();

                int s_id_resultado;
                s_id_resultado = Convert.ToInt32(cmd.Parameters["OFEstadoOper"].Value.ToString());

                return s_id_resultado;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public string ObtenerIdUsuario(string Login)
        {
            try
            {
                conn = new OracleConnection(cadenaConexion);
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = Package + ".SPRKERB_OBTENER_ID_USUARIO";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter isLogin = new OracleParameter("ISLogin", OracleDbType.Varchar2);
                isLogin.Value = Login;
                cmd.Parameters.Add(isLogin);

                OracleParameter ocResult = new OracleParameter();
                ocResult.OracleDbType = OracleDbType.RefCursor;
                ocResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocResult);

                OracleDataReader reader = cmd.ExecuteReader();
                string resultado = "";

                while (reader.Read())
                {
                    resultado = reader["CUSR_ID"].ToString() + "|" + reader["SPFL_NOMBRE"].ToString();
                }

                CerrarConexion(cmd);
                return resultado;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public void CerrarConexion(OracleCommand cmd)
        {
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
        }
    }
}
