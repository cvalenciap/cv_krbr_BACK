using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.BL.Components
{
    public interface IUsuarios
    {
        List<Usuario> buscarUsuarios(string ApePaterno, string ApeMaterno, string Nombre, string Login, string Estado);

        Usuario obtenerUsuario(int Id);

        Usuario crearUsuario(Usuario usuario);

        Usuario actualizarUsuario(Usuario usuario);

        RespuestaOperacionServicio cambiarEstadoUsuario(Usuario usuario);

        RespuestaOperacionServicio cambiarEstadosUsuarios(List<int> IdUsuarios, int Estado, int idUsuario);

        string obtenerIdUsuario(string Login);
    }
}
