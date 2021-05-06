using System;
using System.Collections.Generic;
using KERBERO.WCF.ServiceContracts;
using KERBERO.DL.Repository;
using KERBERO.Util;

namespace KERBERO.BL.Components
{
    public class Auditoria : IAuditoria
    {
        private IAuditoriaRepositorio AuditoriaBD;
        private string strCadenaConexion;

        public Auditoria()
        {
            // obtener cadena de conexión desde el registro para SistemasBD
            strCadenaConexion = RegistroWindows.ObtenerCadenaRegistro(Configuracion.RutaKerbero, Configuracion.ClaveConnectionString);
            //strCadenaConexion = "User Id=FMV_Cerbero; Password=karma; Data Source=(DESCRIPTION =    (ADDRESS = (PROTOCOL = TCP)(HOST = 10.100.120.135)(PORT = 1523))    (CONNECT_DATA =      (SERVER = DEDICATED)      (SERVICE_NAME =DESARROLLO)    ) );";
            if (strCadenaConexion.Equals(string.Empty))
            {
                throw new Exception("No se encuentra la cadena de conexión a la base de datos");
            }
            this.AuditoriaBD = new AuditoriaRepositorio(strCadenaConexion);
        }

        public List<Evento> buscarAuditoria(string CodSistema, string Usuario, string FechaInicio, string FechaFin)
        {
            List<Evento> lstLog = new List<Evento>();
            lstLog = AuditoriaBD.ConsultarLog(CodSistema, Usuario, FechaInicio, FechaFin);
            return lstLog;
        }
    }
}
