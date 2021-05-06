using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.BL.Components
{
    public interface IPermisos
    {
        List<Permiso> buscarPermisos(int IdSistema, string Codigo, string Nombre, string Estado);

        Permiso obtenerPermiso(int Id);

        Permiso crearPermiso(Permiso permiso);

        Permiso actualizarPermiso(Permiso permiso);

        RespuestaOperacionServicio cambiarEstadoPermiso(Permiso permiso);

        RespuestaOperacionServicio cambiarEstadosPermisos(List<int> IdPermisos, int Estado, int idUsuario);
    }
}
