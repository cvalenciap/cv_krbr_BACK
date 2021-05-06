using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.BL.Components
{
    public interface IParametros
    {
        List<Parametro> buscarParametros(string ParamPadre, string Estado);

        Parametro obtenerParametro(int Id);

        Parametro valorParametro(int Id);

        Parametro crearParametro(Parametro parametro);

        Parametro actualizarParametro(Parametro parametro);
    }
}
