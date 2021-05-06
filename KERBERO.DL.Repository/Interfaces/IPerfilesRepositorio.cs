using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;
using Oracle.ManagedDataAccess.Client;

namespace KERBERO.DL.Repository
{
    public interface IPerfilesRepositorio
    {
        /// <summary>
        /// Devuelve una lista de los perfiles de un sistema
        /// </summary>
        /// <returns>lista de entidades Perfil</returns>
        List<Perfil> BuscarPerfiles(int IDSistema, string Nombre, string Estado);

        /// <summary>
        /// Devuelve un perfil con el ID enviado
        /// </summary>
        /// <returns>entidad Perfil</returns>
        Perfil ObtenerPerfil(int Id);

        /// <summary>
        /// Crea un perfil
        /// </summary>
        /// <returns>entidad Perfil</returns>
        Perfil CrearPerfil(OracleCommand cmd, Perfil ObjPerfil);

        /// <summary>
        /// Actualiza los datos de un perfil
        /// </summary>
        /// <returns>entidad Perfil</returns>
        int ActualizarPerfil(OracleCommand cmd, Perfil ObjPerfil);

        /// <summary>
        /// Cabia el estado de un perfil
        /// </summary>
        /// <returns>entero resultado</returns>
        int CambiarEstadoPerfil(OracleCommand cmd, Perfil ObjPerfil);

        /// <summary>
        /// Crea un permiso para un perfil
        /// </summary>
        /// <returns>entero resultado</returns>
        int MantenimientoPermisoXPerfil(OracleCommand cmd, int IDPerfil, int IDPermiso, int Usuario, int Estado);
    }
}
