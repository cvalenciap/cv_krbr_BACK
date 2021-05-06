using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KERBERO.WCF.ServiceContracts;
using Oracle.ManagedDataAccess.Client;

namespace KERBERO.DL.Repository
{
    public interface IParametrosRepositorio
    {
        /// <summary>
        /// Devuelve una lista de parámetros
        /// </summary>
        /// <returns>lista de entidades Parametro</returns>
        List<Parametro> BuscarParametros(string ParamPadre, string Estado);

        /// <summary>
        /// Devuelve un arámetro con el ID enviado
        /// </summary>
        /// <returns>entidad Parametro</returns>

        Parametro ObtenerParametro(int Id);
        /// <summary>
        /// Devuelve un arámetro con el ID enviado
        /// </summary>
        /// <returns>entidad Parametro</returns>
        Parametro ValorParametro(int Id);

        /// <summary>
        /// Crea un parámetro
        /// </summary>
        /// <returns>entidad Parametro</returns>
        Parametro CrearParametro(OracleCommand cmd, Parametro ObjParametro);

        /// <summary>
        /// Actualiza los datos de un parámetro
        /// </summary>
        /// <returns>entidad Parametro</returns>
        int ActualizarParametro(OracleCommand cmd, Parametro ObjParametro);
    }
}
