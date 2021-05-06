using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;
using Oracle.ManagedDataAccess.Client;

namespace KERBERO.DL.Repository
{
    public interface IPermisosRepositorio
    {
        /// <summary>
        /// Devuelve una lista de los permisos de un sistema
        /// </summary>
        /// <returns>lista de entidades Permiso</returns>
        List<Permiso> BuscarPermisos(int IDSistema, string Codigo, string Nombre, string Estado);

        /// <summary>
        /// Devuelve un permiso con el ID enviado
        /// </summary>
        /// <returns>entidad Permiso</returns>
        Permiso ObtenerPermiso(int Id);

        /// <summary>
        /// Crea un permiso
        /// </summary>
        /// <returns>entidad Permiso</returns>
        Permiso CrearPermiso(OracleCommand cmd, Permiso ObjPermiso);

        /// <summary>
        /// Actualiza los datos de un permiso
        /// </summary>
        /// <returns>entidad Permiso</returns>
        int ActualizarPermiso(OracleCommand cmd, Permiso ObjPermiso);

        /// <summary>
        /// Cabia el estado de un permiso
        /// </summary>
        /// <returns>entero resultado</returns>
        int CambiarEstadoPermiso(OracleCommand cmd, Permiso ObjPermiso);
    }
}
