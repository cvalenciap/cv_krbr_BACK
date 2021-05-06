using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.BL.Components
{
    public interface ISistemas
    {
        List<Sistema> buscarSistemas(string Nombre, string Estado);

        Sistema obtenerSistema(int Id);

        Sistema crearSistema(Sistema sistema);

        Sistema actualizarSistema(Sistema sistema);

        RespuestaOperacionServicio cambiarEstadoSistema(Sistema sistema);

        RespuestaOperacionServicio cambiarEstadosSistemas(List<int> IdSistemas, int Estado, int idUsuario);
    }
}
