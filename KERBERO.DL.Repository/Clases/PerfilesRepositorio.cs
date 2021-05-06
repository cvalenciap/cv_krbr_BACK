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
    public class PerfilesRepositorio : IPerfilesRepositorio
    {
        private OracleConnection conn;
        private string Package;
        public const int RESULTADO_OK = 1;
        public const int RESULTADO_ERROR = -1;
        public string cadenaConexion;

        public PerfilesRepositorio(string strCadenaConexion)
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

        public List<Perfil> BuscarPerfiles(int IDSistema, string Nombre, string Estado)
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
                cmd.CommandText = Package + ".SPRKERB_BUSCAR_PERFILES";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter isIDSistema = new OracleParameter("IFIdSist", OracleDbType.Int32);
                isIDSistema.Value = IDSistema;
                cmd.Parameters.Add(isIDSistema);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isEstado = new OracleParameter("ISEstado", OracleDbType.Varchar2);
                isEstado.Value = Estado;
                cmd.Parameters.Add(isEstado);

                OracleParameter ocPerfiles = new OracleParameter();
                ocPerfiles.OracleDbType = OracleDbType.RefCursor;
                ocPerfiles.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPerfiles);

                OracleDataReader reader = cmd.ExecuteReader();
                List<Perfil> ListaPerfiles = new List<Perfil>();
                Perfil perfil;

                while (reader.Read())
                {
                    perfil = new Perfil();
                    perfil.Id = Convert.ToInt32(reader["CPFL_ID"].ToString());
                    perfil.Codigo = reader["SPFL_CODIGO"].ToString();
                    perfil.Nombre = reader["SPFL_NOMBRE"].ToString();
                    perfil.Estado = Convert.ToInt32(reader["FPFL_ESTADO"].ToString());
                    perfil.FechaCreacion = Convert.ToDateTime(reader["DAUD_CREAR_FECHA"].ToString()).ToShortDateString();
                    ListaPerfiles.Add(perfil);
                }
                CerrarConexion(cmd);
                return ListaPerfiles;
            }
            catch
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw new Exception("Error al buscar perfiles");
            }
        }

        public Perfil ObtenerPerfil(int Id)
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
                cmd.CommandText = Package + ".SPRKERB_OBTENER_PERFIL";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = Id;
                cmd.Parameters.Add(inID);

                OracleParameter ocPerfil = new OracleParameter();
                ocPerfil.OracleDbType = OracleDbType.RefCursor;
                ocPerfil.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPerfil);
                
                OracleParameter ocPermisos = new OracleParameter();
                ocPermisos.OracleDbType = OracleDbType.RefCursor;
                ocPermisos.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPermisos);

                OracleParameter ocUsuarios = new OracleParameter();
                ocUsuarios.OracleDbType = OracleDbType.RefCursor;
                ocUsuarios.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocUsuarios);

                OracleDataReader reader = cmd.ExecuteReader();
                Perfil perfil = new Perfil();

                while (reader.Read())
                {
                    perfil.Id = Convert.ToInt32(reader["CPFL_ID"].ToString());
                    perfil.Sistema = new Sistema();
                    perfil.Sistema.Id = Convert.ToInt32(reader["CSIS_ID"].ToString());
                    perfil.Sistema.Nombre = reader["SSIS_NOMBRE"].ToString();
                    perfil.Codigo = reader["SPFL_CODIGO"].ToString();
                    perfil.Nombre = reader["SPFL_NOMBRE"].ToString();
                    perfil.Descripcion = reader["SPFL_DESCRIPCION"].ToString();
                    perfil.Estado = Convert.ToInt32(reader["FPFL_ESTADO"].ToString()); 
                    perfil.HorarioAcceso = reader["SPFL_ACCESOHORARIO"].ToString();
                    perfil.Permisos = new List<Permiso>();
                    perfil.Usuarios = new List<Usuario>();
                }

                //Permisos del Perfil
                reader.NextResult();
                while (reader.Read())
                {
                    Permiso permisoPerf = new Permiso();
                    permisoPerf.Id = Convert.ToInt32(reader["CPRM_ID"].ToString());
                    permisoPerf.Estado = Convert.ToInt32(reader["FPRM_ESTADO"].ToString());
                    permisoPerf.Codigo = reader["SPRM_CODIGO"].ToString();
                    permisoPerf.Nombre = reader["SPRM_NOMBRE"].ToString();
                    permisoPerf.Descripcion = reader["SPRM_DESCRIPCION"].ToString();
                    perfil.Permisos.Add(permisoPerf);
                }

                //Usuarios del perfil
                reader.NextResult();
                while (reader.Read())
                {
                    Usuario UsuarioPerf = new Usuario();
                    UsuarioPerf.Id = Convert.ToInt32(reader["CUSR_ID"].ToString());
                    UsuarioPerf.Estado = Convert.ToInt32(reader["FUSR_ESTADO"].ToString());
                    UsuarioPerf.Login = reader["SUSR_LOGIN"].ToString();
                    UsuarioPerf.Nombre = reader["SUSR_NOMBRE"].ToString();
                    perfil.Usuarios.Add(UsuarioPerf);
                }

                CerrarConexion(cmd);
                return perfil;
            }
            catch
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw new Exception("Error al obtener perfil");
            }
        }

        public Perfil CrearPerfil(OracleCommand cmd, Perfil ObjPerfil)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_INSERTAR_PERFIL";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inIDSistema = new OracleParameter("IFIdSist", OracleDbType.Int32);
                inIDSistema.Value = ObjPerfil.Sistema.Id;
                cmd.Parameters.Add(inIDSistema);

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = ObjPerfil.Codigo;
                cmd.Parameters.Add(isCodigo);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = ObjPerfil.Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isDescr = new OracleParameter("ISDescr", OracleDbType.Varchar2);
                isDescr.Value = ObjPerfil.Descripcion;
                cmd.Parameters.Add(isDescr);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjPerfil.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter isHorario = new OracleParameter("ISHorario", OracleDbType.Varchar2);
                isHorario.Value = ObjPerfil.HorarioAcceso;
                cmd.Parameters.Add(isHorario);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjPerfil.Usuario;
                cmd.Parameters.Add(inUsuario);

                //OracleParameter ocEstadoOper = new OracleParameter("iintEstadoOper", OracleDbType.Int32);
                //ocEstadoOper.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(ocEstadoOper);

                OracleParameter ocIDGenerado = new OracleParameter("OFId", OracleDbType.Int32);
                ocIDGenerado.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocIDGenerado);

                cmd.ExecuteNonQuery();

                //int s_id_resultado = Convert.ToInt32(cmd.Parameters["iintEstadoOper"].Value.ToString());
                ObjPerfil.Id = Convert.ToInt32(cmd.Parameters["OFId"].Value.ToString());
                //CerrarConexion(cmd);

                return ObjPerfil;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int ActualizarPerfil(OracleCommand cmd, Perfil ObjPerfil)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_ACTUALIZAR_PERFIL";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjPerfil.Id;
                cmd.Parameters.Add(inID);

                OracleParameter inIDSistema = new OracleParameter("IFIdSist", OracleDbType.Int32);
                inIDSistema.Value = ObjPerfil.Sistema.Id;
                cmd.Parameters.Add(inIDSistema);

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = ObjPerfil.Codigo;
                cmd.Parameters.Add(isCodigo);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = ObjPerfil.Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isDescr = new OracleParameter("ISDescr", OracleDbType.Varchar2);
                isDescr.Value = ObjPerfil.Descripcion;
                cmd.Parameters.Add(isDescr);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjPerfil.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter isHorario = new OracleParameter("ISHorario", OracleDbType.Varchar2);
                isHorario.Value = ObjPerfil.HorarioAcceso;
                cmd.Parameters.Add(isHorario);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjPerfil.Usuario;
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

        public int CambiarEstadoPerfil(OracleCommand cmd, Perfil ObjPerfil)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_CAMBIAR_ESTADO_PERFIL";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjPerfil.Id;
                cmd.Parameters.Add(inID);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjPerfil.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjPerfil.Usuario;
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

        public int MantenimientoPermisoXPerfil(OracleCommand cmd, int IDPerfil, int IDPermiso, int Usuario, int Estado)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_MANT_PERMXPERF";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inIDPermiso = new OracleParameter("IFIdPerm", OracleDbType.Varchar2);
                inIDPermiso.Value = IDPermiso;
                cmd.Parameters.Add(inIDPermiso);

                OracleParameter inIDPerfil = new OracleParameter("IFIdPerf", OracleDbType.Varchar2);
                inIDPerfil.Value = IDPerfil;
                cmd.Parameters.Add(inIDPerfil);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Varchar2);
                inUsuario.Value = Usuario;
                cmd.Parameters.Add(inUsuario);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Varchar2);
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

        public void CerrarConexion(OracleCommand cmd)
        {
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
        }
    }
}
