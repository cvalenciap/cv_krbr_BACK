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
    public class Parametros : IParametros
    {
        private IParametrosRepositorio ParametrosBD;
        private string strCadenaConexion;

        public Parametros()
        {
            // obtener cadena de conexión desde el registro para SistemasBD
            strCadenaConexion = RegistroWindows.ObtenerCadenaRegistro(Configuracion.RutaKerbero, Configuracion.ClaveConnectionString);
            //strCadenaConexion = "User Id=FMV_Cerbero; Password=karma; Data Source=(DESCRIPTION =    (ADDRESS = (PROTOCOL = TCP)(HOST = 10.100.120.135)(PORT = 1523))    (CONNECT_DATA =      (SERVER = DEDICATED)      (SERVICE_NAME =DESARROLLO)    ) );";
            if (strCadenaConexion.Equals(string.Empty))
            {
                throw new Exception("No se encuentra la cadena de conexión a la base de datos");
            }
            this.ParametrosBD = new ParametrosRepositorio(strCadenaConexion);
        }

        public List<Parametro> buscarParametros(string Nombre, string Estado)
        {
            List<Parametro> lstParametros = new List<Parametro>();
            lstParametros = ParametrosBD.BuscarParametros(Nombre, Estado);
            return lstParametros;
        }

        public Parametro obtenerParametro(int Id)
        {
            Parametro objParametro = new Parametro();
            objParametro = ParametrosBD.ObtenerParametro(Id);
            return objParametro;
        }

        public Parametro valorParametro(int Id)
        {
            Parametro objParametro = new Parametro();
            objParametro = ParametrosBD.ValorParametro(Id);
            return objParametro;
        }

        public Parametro crearParametro(Parametro parametro)
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
                        parametro = ParametrosBD.CrearParametro(cmd, parametro);
                        if (parametro.Id < 0)
                        {
                            throw new Exception("Ocurrió un error al guardar el parametro");
                        }
                        trx.Commit();
                    }
                    catch
                    {
                        trx.Rollback();
                        throw new Exception("Ocurrió un error al guardar el parametro");
                    }
                }
            }
            CerrarConexion(conn);
            return parametro;
        }

        public Parametro actualizarParametro(Parametro parametro)
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
                        int resultParamero = ParametrosBD.ActualizarParametro(cmd, parametro);
                        if (resultParamero < 0)
                        {
                            throw new Exception("Ocurrió un error al actualizar el parametro");
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
            return parametro;
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
