using KERBERO.WCF.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace KERBERO.DL.Repository
{
    /// <summary>
    /// Abstracción de base de datos
    /// </summary>
    public interface ISistemasRepositorio
    {
        /// <summary>
        /// Devuelve una lista de los sistemas activos que controla Cerbero
        /// </summary>
        /// <returns>lista de entidades Sistema</returns>
        List<Sistema> BuscarSistemas(string Nombre, string Estado);

        /// <summary>
        /// Devuelve un sistema con el ID enviado
        /// </summary>
        /// <returns>entidad Sistema</returns>
        Sistema ObtenerSistema(int Id);

        /// <summary>
        /// Crea un sistema
        /// </summary>
        /// <returns>entidad Sistema</returns>
        Sistema CrearSistema(OracleCommand cmd, Sistema ObjSistema);

        /// <summary>
        /// Actualiza los datos de un sistema
        /// </summary>
        /// <returns>entidad Sistema</returns>
        int ActualizarSistema(OracleCommand cmd, Sistema ObjSistema);

        /// <summary>
        /// Cabia el estado de un sistema
        /// </summary>
        /// <returns>entero resultado</returns>
        int CambiarEstadoSistema(OracleCommand cmd, Sistema ObjSistema);

        /// <summary>
        /// Actualiza la versión del sistema
        /// </summary>
        /// <returns>entero resultado</returns>
        int ActualizarVersionSistema(OracleCommand cmd, int IdSistema, string Version);
    }
}
