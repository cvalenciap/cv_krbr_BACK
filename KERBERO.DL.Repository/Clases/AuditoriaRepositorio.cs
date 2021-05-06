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
    public class AuditoriaRepositorio : IAuditoriaRepositorio
    {
        private OracleConnection conn;
        private string Package;
        public const int RESULTADO_OK = 1;
        public const int RESULTADO_ERROR = -1;
        public string cadenaConexion;

        public AuditoriaRepositorio(string strCadenaConexion)
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

        public List<Evento> ConsultarLog(string CodSistema, string Usuario, string FechaInicio, string FechaFin)
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
                cmd.CommandText = Package + ".SPRKERB_CONSULTAR_LOG";
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter isCodSistema = new OracleParameter("ISCodSist", OracleDbType.Varchar2);
                isCodSistema.Value = CodSistema;
                cmd.Parameters.Add(isCodSistema);

                OracleParameter isUsuario = new OracleParameter("ISUsuario", OracleDbType.Varchar2);
                isUsuario.Value = Usuario;
                cmd.Parameters.Add(isUsuario);

                OracleParameter idFechaInicio = new OracleParameter("ISFecInicio", OracleDbType.Varchar2);
                idFechaInicio.Value = FechaInicio;
                cmd.Parameters.Add(idFechaInicio);

                OracleParameter idFechaFin = new OracleParameter("ISFecFin", OracleDbType.Varchar2);
                idFechaFin.Value = FechaFin;
                cmd.Parameters.Add(idFechaFin);

                OracleParameter ocAuditoria = new OracleParameter();
                ocAuditoria.OracleDbType = OracleDbType.RefCursor;
                ocAuditoria.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ocAuditoria);

                OracleDataReader reader = cmd.ExecuteReader();
                List<Evento> ListaEventos = new List<Evento>();
                Evento evento;

                while (reader.Read())
                {
                    evento = new Evento();
                    evento.Id = Convert.ToInt32(reader["CAUD_ID"].ToString());
                    evento.Sistema = reader["SSIS_NOMBRE"].ToString();
                    evento.Usuario = reader["SUSR_LOGIN"].ToString();
                    evento.Descripcion = reader["SAUD_DESCRIPCION"].ToString();
                    evento.FechaCreacion = Convert.ToDateTime(reader["DAUD_FECHA_CREACION_LOG"].ToString());
                    evento.Origen = reader["SUSR_CREACION"].ToString();
                    ListaEventos.Add(evento);
                }
                CerrarConexion(cmd);
                return ListaEventos;
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
