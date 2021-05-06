using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;
using Oracle.ManagedDataAccess.Client;

namespace KERBERO.DL.Repository
{
    public interface IUsuariosRepositorio
    {
        /// <summary>
        /// Devuelve una lista de usuarios
        /// </summary>
        /// <returns>lista de entidades Usuario</returns>
        List<Usuario> BuscarUsuarios(string ApePaterno, string ApeMaterno, string Nombre, string Login, string Estado);

        /// <summary>
        /// Devuelve un usuario con el ID enviado
        /// </summary>
        /// <returns>entidad Usuario</returns>
        Usuario ObtenerUsuario(int Id);

        /// <summary>
        /// Crea un usuario
        /// </summary>
        /// <returns>entidad Usuario</returns>
        Usuario CrearUsuario(OracleCommand cmd, Usuario ObjUsuario);

        /// <summary>
        /// Actualiza los datos de un usuario
        /// </summary>
        /// <returns>entidad Usuario</returns>
        int ActualizarUsuario(OracleCommand cmd, Usuario ObjUsuario);

        /// <summary>
        /// Cabia el estado de un usuario
        /// </summary>
        /// <returns>entero resultado</returns>
        int CambiarEstadoUsuario(OracleCommand cmd, Usuario ObjUsuario);

        /// <summary>
        /// Crea un permiso para un usuario
        /// </summary>
        /// <returns>entero resultado</returns>
        int MantenimientoUsuarioXPermiso(OracleCommand cmd, int IDUsuario, int IDPermiso, int Usuario, int Estado);

        /// <summary>
        /// Crea un perfil para un usuario
        /// </summary>
        /// <returns>entero resultado</returns>
        int MantenimientoUsuarioXPerfil(OracleCommand cmd, int IDUsuario, int IDPerfil, int Usuario, int Estado);

        /// <summary>
        /// Actualiza o crea contraseña del usuario
        /// </summary>
        /// <returns>entero resultado</returns>
        int ActualizarContrasena(OracleCommand cmd, int IDUsuario, string Contrasena, int Usuario);

        /// <summary>
        /// Devuelve el id de Usuario
        /// </summary>
        /// <returns>int IdUsuario</returns>
        string ObtenerIdUsuario(string Login);
    }
}
