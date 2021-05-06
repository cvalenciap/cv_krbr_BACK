using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.DL.Repository
{
    public class ParametrosRepositorio : IParametrosRepositorio
    {
        private OracleConnection conn;
        private string Package;
        public const int RESULTADO_OK = 1;
        public const int RESULTADO_ERROR = -1;
        public string cadenaConexion;

        public ParametrosRepositorio(string strCadenaConexion)
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

        public List<Parametro> BuscarParametros(string ParamPadre, string Estado)
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
                cmd.CommandText = Package + ".SPRKERB_BUSCAR_PARAMETROS";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter isParamPadre = new OracleParameter("ISParamPadre", OracleDbType.Varchar2);
                isParamPadre.Value = ParamPadre;
                cmd.Parameters.Add(isParamPadre);

                OracleParameter isEstado = new OracleParameter("ISEstado", OracleDbType.Varchar2);
                isEstado.Value = Estado;
                cmd.Parameters.Add(isEstado);

                OracleParameter ocUsuarios = new OracleParameter();
                ocUsuarios.OracleDbType = OracleDbType.RefCursor;
                ocUsuarios.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocUsuarios);

                OracleDataReader reader = cmd.ExecuteReader();
                List<Parametro> ListaParametros = new List<Parametro>();
                Parametro parametro;

                while (reader.Read())
                {
                    parametro = new Parametro();
                    parametro.Id = Convert.ToInt32(reader["CPAR_IDTABLA"].ToString());
                    parametro.Valor = reader["SPAR_VALOR"].ToString();
                    parametro.IdPadre = Convert.ToInt32(reader["NPAR_IDTABLAPADRE"].ToString());
                    parametro.ValorPadre = reader["SPAR_VALOR_PADRE"].ToString(); 
                    parametro.Codigo = reader["SPAR_CODIGO"].ToString();
                    parametro.Estado = Convert.ToInt32(reader["FPAR_ESTADO"].ToString());
                    parametro.FechaRegistro = Convert.ToDateTime(reader["DAUD_CREAR_FECHA"].ToString()).ToShortDateString(); 
                    ListaParametros.Add(parametro);
                }
                CerrarConexion(cmd);
                return ListaParametros;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public Parametro ObtenerParametro(int Id)
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
                cmd.CommandText = Package + ".SPRKERB_OBTENER_PARAMETRO";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = Id;
                cmd.Parameters.Add(inID);

                OracleParameter ocParametro = new OracleParameter();
                ocParametro.OracleDbType = OracleDbType.RefCursor;
                ocParametro.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocParametro);

                OracleDataReader reader = cmd.ExecuteReader();
                Parametro parametro = new Parametro();

                while (reader.Read())
                {
                    parametro.Id = Convert.ToInt32(reader["CPAR_IDTABLA"].ToString());
                    parametro.Codigo = reader["SPAR_CODIGO"].ToString();
                    parametro.Valor = reader["SPAR_VALOR"].ToString();
                    if (DBNull.Value == reader["NPAR_IDTABLAPADRE"])
                    {
                        parametro.IdPadre = 0;
                        parametro.ValorPadre = "";
                    }
                    else
                    {
                        parametro.IdPadre = Convert.ToInt32(reader["NPAR_IDTABLAPADRE"].ToString());
                        parametro.ValorPadre = reader["SPAR_VALOR_PADRE"].ToString(); 
                    }
                    parametro.Estado = Convert.ToInt32(reader["FPAR_ESTADO"].ToString());
                }

                CerrarConexion(cmd);
                return parametro;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public Parametro ValorParametro(int Id)
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
                cmd.CommandText = Package + ".SPRKERB_VALOR_PARAMETRO";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = Id;
                cmd.Parameters.Add(inID);

                OracleParameter ocParametro = new OracleParameter();
                ocParametro.OracleDbType = OracleDbType.RefCursor;
                ocParametro.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocParametro);

                OracleDataReader reader = cmd.ExecuteReader();
                Parametro parametro = new Parametro();

                while (reader.Read())
                {
                    parametro.Id = Convert.ToInt32(reader["CPAR_IDTABLA"].ToString());
                    parametro.Valor = reader["SPAR_VALOR"].ToString();
                    parametro.Estado = Convert.ToInt32(reader["FPAR_ESTADO"].ToString());
                    parametro.Codigo = reader["SPAR_CODIGO"].ToString();
                }

                CerrarConexion(cmd);
                return parametro;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public Parametro CrearParametro(OracleCommand cmd, Parametro ObjParametro)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_INSERTAR_PARAMETRO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                OracleParameter isValor = new OracleParameter("ISValor", OracleDbType.Varchar2);
                isValor.Value = ObjParametro.Valor.Trim();
                cmd.Parameters.Add(isValor);

                OracleParameter inIDPadre = new OracleParameter("IFIdPadre", OracleDbType.Int32);
                inIDPadre.Value = ObjParametro.IdPadre;
                cmd.Parameters.Add(inIDPadre);

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = ObjParametro.Codigo.Trim();
                cmd.Parameters.Add(isCodigo);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjParametro.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjParametro.Usuario;
                cmd.Parameters.Add(inUsuario);

                //OracleParameter ocEstadoOper = new OracleParameter("iintEstadoOper", OracleDbType.Int32);
                //ocEstadoOper.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(ocEstadoOper);

                OracleParameter ocIDGenerado = new OracleParameter("OFId", OracleDbType.Int32);
                ocIDGenerado.Direction = ParameterDirection.Output;
                ocIDGenerado.Size = 1;
                cmd.Parameters.Add(ocIDGenerado);
                
                cmd.ExecuteNonQuery();

                //int s_id_resultado = Convert.ToInt32(cmd.Parameters["iintEstadoOper"].Value.ToString());
                ObjParametro.Id = Convert.ToInt32(cmd.Parameters["OFId"].Value.ToString());

                return ObjParametro;
            }
            catch (Exception ex)
            {
                //RegistroEventos.RegistrarError(ex, RegistroEventos.FUENTE_REPO_BD);
                throw ex;
            }
        }

        public int ActualizarParametro(OracleCommand cmd, Parametro ObjParametro)
        {
            try
            {
                cmd.CommandText = Package + ".SPRKERB_ACTUALIZAR_PARAMETRO";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter inID = new OracleParameter("IFId", OracleDbType.Int32);
                inID.Value = ObjParametro.Id;
                cmd.Parameters.Add(inID);

                OracleParameter isValor = new OracleParameter("ISValor", OracleDbType.Varchar2);
                isValor.Value = ObjParametro.Valor;
                cmd.Parameters.Add(isValor);

                OracleParameter inEstado = new OracleParameter("IFEstado", OracleDbType.Int32);
                inEstado.Value = ObjParametro.Estado;
                cmd.Parameters.Add(inEstado);

                OracleParameter inUsuario = new OracleParameter("IFUsuario", OracleDbType.Int32);
                inUsuario.Value = ObjParametro.Usuario;
                cmd.Parameters.Add(inUsuario);

                OracleParameter isCodigo = new OracleParameter("ISCodigo", OracleDbType.Varchar2);
                isCodigo.Value = ObjParametro.Codigo;
                cmd.Parameters.Add(isCodigo);

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
