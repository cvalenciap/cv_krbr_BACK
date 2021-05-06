using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.BL.Components
{
    public interface IPerfiles
    {
        List<Perfil> buscarPerfiles(int IdSistema, string Nombre, string Estado);

        Perfil obtenerPerfil(int Id);

        Perfil crearPerfil(Perfil perfil);

        Perfil actualizarPerfil(Perfil perfil);

        RespuestaOperacionServicio cambiarEstadoPerfil(Perfil perfil);

        RespuestaOperacionServicio cambiarEstadosPerfiles(List<int> IdPerfiles, int Estado, int idUsuario);
    }
}
