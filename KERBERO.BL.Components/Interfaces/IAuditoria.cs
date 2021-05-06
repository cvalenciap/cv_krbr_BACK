using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.BL.Components
{
    public interface IAuditoria
    {
        List<Evento> buscarAuditoria(string CodSistema, string Usuario, string FechaInicio, string FechaFin);
    }
}
