using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace KERBERO.DL.Repository
{
    public class SistemasRepositorio : ISistemasRepositorio
    {
        private OracleConnection conn;
        private string Package;
        public const int RESULTADO_OK = 1;       
        public const int RESULTADO_ERROR = -1;
        public string cadenaConexion;

        public SistemasRepositorio(string strCadenaConexion)
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
        
        public List<Sistema> BuscarSistemas(string Nombre, string Estado)
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
                cmd.CommandText = Package + ".SPRKERB_BUSCAR_SISTEMAS";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isEstado = new OracleParameter("ISEstado", OracleDbType.Varchar2);
                isEstado.Value = Estado;
                cmd.Parameters.Add(isEstado);

                OracleParameter ocSistemas = new OracleParameter();
                ocSistemas.OracleDbType = OracleDbType.RefCursor;
                ocSistemas.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocSistemas);
                
                OracleDataReader reader = cmd.ExecuteReader();
                List<Sistema> ListaSistemas = new List<Sistema>();
                Sistema sistema;

                while (reader.Read())
                {
                    sistema = new Sistema();
                    sistema.Id = Convert.ToInt32(reader["CSIS_ID"].ToString());
                    sistema.Codigo = reader["SSIS_CODIGO"].ToString();
                    sistema.Nombre = reader["SSIS_NOMBRE"].ToString();
                    sistema.Descripcion = reader["SSIS_DESCRIPCION"].ToString();
                    sistema.Version = reader["SSIS_VERSION"].ToString();
                    sistema.Estado = Convert.ToInt32(reader["FSIS_ESTADO"].ToString());
                    sistema.FechaRegistro = Convert.ToDateTime(reader["DAUD_CREAR_FECHA"].ToString()).ToShortDateString();
                    ListaSistemas.Add(sistema);
                }
                CerrarConexion(cmd);
                return ListaSistemas;
            }
            catch
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw new Exception("Error al buscar sistemas");
            }
        }

        public Sistema ObtenerSistema(int Id)
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
                cmd.CommandText = Package + ".SPRKERB_OBTENER_SISTEMA";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = Id;
                cmd.Parameters.Add(inID);

                OracleParameter ocSistema = new OracleParameter();
                ocSistema.OracleDbType = OracleDbType.RefCursor;
                ocSistema.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocSistema);

                OracleParameter ocVersiones = new OracleParameter();
                ocVersiones.OracleDbType = OracleDbType.RefCursor;
                ocVersiones.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocVersiones);

                OracleParameter ocPermisos = new OracleParameter();
                ocPermisos.OracleDbType = OracleDbType.RefCursor;
                ocPermisos.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPermisos);

                OracleParameter ocPerfiles = new OracleParameter();
                ocPerfiles.OracleDbType = OracleDbType.RefCursor;
                ocPerfiles.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocPerfiles);

                OracleDataReader reader = cmd.ExecuteReader();
                Sistema sistema = new Sistema();

                while (reader.Read())
                {
                    sistema.Id = Convert.ToInt32(reader["CSIS_ID"].ToString());
                    sistema.Nombre = reader["SSIS_NOMBRE"].ToString();
                    sistema.Codigo = reader["SSIS_CODIGO"].ToString();
                    sistema.Descripcion = reader["SSIS_DESCRIPCION"].ToString();
                    sistema.Version = reader["SSIS_VERSION"].ToString();
                    sistema.Estado = Convert.ToInt32(reader["FSIS_ESTADO"].ToString());
                    sistema.HorarioAcceso = reader["SSIS_ACCESOHORARIO"].ToString();
                    sistema.Versiones = new List<VersionSistema>();
                    sistema.Permisos = new List<Permiso>();
                    sistema.Perfiles = new List<Perfil>();
                }

                //mostrar versión activa y úlltimas 3
                reader.NextResult();
                while (reader.Read())
                {
                    VersionSistema versionSist = new VersionSistema();
                    versionSist.Id = Convert.ToInt32(reader["CSIS_ID"].ToString());
                    versionSist.Estado = Convert.ToInt32(reader["FSIS_ESTADO"].ToString());
                    versionSist.Version = reader["SSIS_VERSION"].ToString();
                    versionSist.CrearFecha = Convert.ToDateTime(reader["DAUD_CREAR_FECHA"].ToString()).ToShortDateString();
                    sistema.Versiones.Add(versionSist);
                }

                //Permisos del sistema
                reader.NextResult();
                while (reader.Read())
                {
                    Permiso permisoSist = new Permiso();
                    permisoSist.Id = Convert.ToInt32(reader["CPRM_ID"].ToString());
                    permisoSist.Estado = Convert.ToInt32(reader["FPRM_ESTADO"].ToString());
                    permisoSist.Codigo = reader["SPRM_CODIGO"].ToString();
                    permisoSist.Nombre = reader["SPRM_NOMBRE"].ToString();
                    permisoSist.Descripcion = reader["SPRM_DESCRIPCION"].ToString();
                    sistema.Permisos.Add(permisoSist);
                }

                //Perfiles del sistema
                reader.NextResult();
                while (reader.Read())
                {
                    Perfil perfilSist = new Perfil();
                    perfilSist.Id = Convert.ToInt32(reader["CPFL_ID"].ToString());
                    perfilSist.Estado = Convert.ToInt32(reader["FPFL_ESTADO"].ToString());
                    perfilSist.Codigo = reader["SPFL_CODIGO"].ToString();
                    perfilSist.Nombre = reader["SPFL_NOMBRE"].ToString();
                    perfilSist.Descripcion = reader["SPFL_DESCRIPCION"].ToString();
                    sistema.Perfiles.Add(perfilSist);
                }

                CerrarConexion(cmd);
                return sistema;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw new Exception("Error al obtener sistema");
            }
        }

        public Sistema CrearSistema(OracleCommand cmd, Sistema ObjSistema)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_INSERTAR_SISTEMA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = ObjSistema.Codigo;
                cmd.Parameters.Add(isCodigo);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = ObjSistema.Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isDescr = new OracleParameter("ISDescr", OracleDbType.Varchar2);
                isDescr.Value = ObjSistema.Descripcion;
                cmd.Parameters.Add(isDescr);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjSistema.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter isHorario = new OracleParameter("ISHorario", OracleDbType.Varchar2);
                isHorario.Value = ObjSistema.HorarioAcceso;
                cmd.Parameters.Add(isHorario);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjSistema.Usuario;
                cmd.Parameters.Add(inUsuario);

                OracleParameter ocIDGenerado = new OracleParameter("OFId", OracleDbType.Int32);
                ocIDGenerado.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocIDGenerado);

                cmd.ExecuteNonQuery();
                
                ObjSistema.Id = Convert.ToInt32(cmd.Parameters["OFId"].Value.ToString());
                
                return ObjSistema;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int ActualizarSistema(OracleCommand cmd, Sistema ObjSistema)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_ACTUALIZAR_SISTEMA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjSistema.Id;
                cmd.Parameters.Add(inID);

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = ObjSistema.Codigo;
                cmd.Parameters.Add(isCodigo);

                OracleParameter isNombre = new OracleParameter("ISNombre", OracleDbType.Varchar2);
                isNombre.Value = ObjSistema.Nombre;
                cmd.Parameters.Add(isNombre);

                OracleParameter isDescr = new OracleParameter("ISDescr", OracleDbType.Varchar2);
                isDescr.Value = ObjSistema.Descripcion;
                cmd.Parameters.Add(isDescr);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjSistema.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter isHorario = new OracleParameter("ISHorario", OracleDbType.Varchar2);
                isHorario.Value = ObjSistema.HorarioAcceso;
                cmd.Parameters.Add(isHorario);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjSistema.Usuario;
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

        public int CambiarEstadoSistema(OracleCommand cmd, Sistema ObjSistema)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_CAMBIAR_ESTADO_SISTEMA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjSistema.Id;
                cmd.Parameters.Add(inID);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjSistema.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjSistema.Usuario;
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

        public int ActualizarVersionSistema(OracleCommand cmd, int IdSistema, string Version)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_ACTUALIZAR_VERSION";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter inIDSistema = new OracleParameter("IFIdSist", OracleDbType.Int32);
                inIDSistema.Value = IdSistema;
                cmd.Parameters.Add(inIDSistema);

                OracleParameter isVersion = new OracleParameter("ISVersion", OracleDbType.Varchar2);
                isVersion.Value = Version;
                cmd.Parameters.Add(isVersion);

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

        public void CerrarConexion(OracleCommand cmd)
        {
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
        }
    }
}
