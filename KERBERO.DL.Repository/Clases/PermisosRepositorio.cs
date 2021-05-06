using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using KERBERO.WCF.ServiceContracts;
using System.Data;

namespace KERBERO.DL.Repository
{
    public class PermisosRepositorio : IPermisosRepositorio
    {
        private OracleConnection conn;
        private string Package;
        public const int RESULTADO_OK = 1;
        public const int RESULTADO_ERROR = -1;
        public string cadenaConexion;

        public PermisosRepositorio(string strCadenaConexion)
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

        public List<Permiso> BuscarPermisos(int IDSistema, string Codigo, string Nombre, string Estado)
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
                cmd.CommandText = Package + ".SPRKERB_BUSCAR_PERMISOS";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter isIDSistema = new OracleParameter("IFIdSist", OracleDbType.Int32);
                isIDSistema.Value = IDSistema;
                cmd.Parameters.Add(isIDSistema);

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = Codigo;
                cmd.Parameters.Add(isCodigo);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isEstado = new OracleParameter("ISEstado", OracleDbType.Varchar2);
                isEstado.Value = Estado;
                cmd.Parameters.Add(isEstado);

                OracleParameter ocPermisos = new OracleParameter();
                ocPermisos.OracleDbType = OracleDbType.RefCursor;
                ocPermisos.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPermisos);

                OracleDataReader reader = cmd.ExecuteReader();
                List<Permiso> ListaPermisos = new List<Permiso>();
                Permiso permiso;

                while (reader.Read())
                {
                    permiso = new Permiso();
                    permiso.Id = Convert.ToInt32(reader["CPRM_ID"].ToString());
                    permiso.Codigo = reader["SPRM_CODIGO"].ToString();
                    permiso.Nombre = reader["SPRM_NOMBRE"].ToString();
                    permiso.Valor = reader["SPRM_VALOR"].ToString();
                    permiso.Estado = Convert.ToInt32(reader["FPRM_ESTADO"].ToString());
                    permiso.FechaCreacion = Convert.ToDateTime(reader["DAUD_CREAR_FECHA"].ToString()).ToShortDateString();
                    ListaPermisos.Add(permiso);
                }
                CerrarConexion(cmd);
                return ListaPermisos;
            }
            catch
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw new Exception("Error al buscar Permisos");
            }
        }

        public Permiso ObtenerPermiso(int Id)
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
                cmd.CommandText = Package + ".SPRKERB_OBTENER_PERMISO";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = Id;
                cmd.Parameters.Add(inID);

                OracleParameter ocPermiso = new OracleParameter();
                ocPermiso.OracleDbType = OracleDbType.RefCursor;
                ocPermiso.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPermiso);

                OracleParameter ocPerfiles = new OracleParameter();
                ocPerfiles.OracleDbType = OracleDbType.RefCursor;
                ocPerfiles.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPerfiles);

                OracleParameter ocUsuarios = new OracleParameter();
                ocUsuarios.OracleDbType = OracleDbType.RefCursor;
                ocUsuarios.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocUsuarios);

                OracleDataReader reader = cmd.ExecuteReader();
                Permiso permiso = new Permiso();

                while (reader.Read())
                {
                    permiso.Id = Convert.ToInt32(reader["CPRM_ID"].ToString());
                    permiso.Sistema = new Sistema();
                    permiso.Sistema.Id = Convert.ToInt32(reader["CSIS_ID"].ToString());
                    permiso.Sistema.Nombre = reader["SSIS_NOMBRE"].ToString();
                    permiso.Codigo = reader["SPRM_CODIGO"].ToString();
                    permiso.Nombre = reader["SPRM_NOMBRE"].ToString();
                    permiso.Valor = reader["SPRM_VALOR"].ToString();
                    permiso.Descripcion = reader["SPRM_DESCRIPCION"].ToString();
                    permiso.Estado = Convert.ToInt32(reader["FPRM_ESTADO"].ToString());
                    permiso.Perfiles = new List<Perfil>();
                    permiso.Usuarios = new List<Usuario>();
                }

                //Perfiles del Permiso
                reader.NextResult();
                while (reader.Read())
                {
                    Perfil perfilPerm = new Perfil();
                    perfilPerm.Id = Convert.ToInt32(reader["CPFL_ID"].ToString());
                    perfilPerm.Estado = Convert.ToInt32(reader["FPFL_ESTADO"].ToString());
                    perfilPerm.Codigo = reader["SPFL_CODIGO"].ToString();
                    perfilPerm.Nombre = reader["SPFL_NOMBRE"].ToString();
                    perfilPerm.Descripcion = reader["SPFL_DESCRIPCION"].ToString();
                    permiso.Perfiles.Add(perfilPerm);
                }

                //Usuarios del Permiso
                reader.NextResult();
                while (reader.Read())
                {
                    Usuario usuarioPerm = new Usuario();
                    usuarioPerm.Id = Convert.ToInt32(reader["CUSR_ID"].ToString());
                    usuarioPerm.Estado = Convert.ToInt32(reader["FUSR_ESTADO"].ToString());
                    usuarioPerm.Login = reader["SUSR_LOGIN"].ToString();
                    usuarioPerm.Nombre = reader["SUSR_NOMBRE"].ToString();
                    permiso.Usuarios.Add(usuarioPerm);
                }

                CerrarConexion(cmd);
                return permiso;
            }
            catch
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw new Exception("Error al obtener Permiso");
            }
        }

        public Permiso CrearPermiso(OracleCommand cmd, Permiso ObjPermiso)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_INSERTAR_PERMISO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inIDSistema = new OracleParameter("IFIdSist", OracleDbType.Int32);
                inIDSistema.Value = ObjPermiso.Sistema.Id;
                cmd.Parameters.Add(inIDSistema);

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = ObjPermiso.Codigo;
                cmd.Parameters.Add(isCodigo);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = ObjPermiso.Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isValor = new OracleParameter("ISValor", OracleDbType.Varchar2);
                isValor.Value = ObjPermiso.Valor;
                cmd.Parameters.Add(isValor);

                OracleParameter isDescr = new OracleParameter("ISDescr", OracleDbType.Varchar2);
                isDescr.Value = ObjPermiso.Descripcion;
                cmd.Parameters.Add(isDescr);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjPermiso.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjPermiso.Usuario;
                cmd.Parameters.Add(inUsuario);

                //OracleParameter ocEstadoOper = new OracleParameter("iintEstadoOper", OracleDbType.Int32);
                //ocEstadoOper.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(ocEstadoOper);

                OracleParameter ocIDGenerado = new OracleParameter("OFId", OracleDbType.Int32);
                ocIDGenerado.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocIDGenerado);

                cmd.ExecuteNonQuery();

                //int s_id_resultado = Convert.ToInt32(cmd.Parameters["iintEstadoOper"].Value.ToString());
                ObjPermiso.Id = Convert.ToInt32(cmd.Parameters["OFId"].Value.ToString());
                //CerrarConexion(cmd);

                return ObjPermiso;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int ActualizarPermiso(OracleCommand cmd, Permiso ObjPermiso)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_ACTUALIZAR_PERMISO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjPermiso.Id;
                cmd.Parameters.Add(inID);

                OracleParameter inIDSistema = new OracleParameter("IFIdSist", OracleDbType.Int32);
                inIDSistema.Value = ObjPermiso.Sistema.Id;
                cmd.Parameters.Add(inIDSistema);

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = ObjPermiso.Codigo;
                cmd.Parameters.Add(isCodigo);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = ObjPermiso.Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isValor = new OracleParameter("ISValor", OracleDbType.Varchar2);
                isValor.Value = ObjPermiso.Valor;
                cmd.Parameters.Add(isValor);

                OracleParameter isDescr = new OracleParameter("ISDescr", OracleDbType.Varchar2);
                isDescr.Value = ObjPermiso.Descripcion;
                cmd.Parameters.Add(isDescr);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjPermiso.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjPermiso.Usuario;
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

        public int CambiarEstadoPermiso(OracleCommand cmd, Permiso ObjPermiso)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_CAMBIAR_ESTADO_PERMISO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjPermiso.Id;
                cmd.Parameters.Add(inID);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjPermiso.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjPermiso.Usuario;
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

        public void CerrarConexion(OracleCommand cmd)
        {
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
        }
    }
}
