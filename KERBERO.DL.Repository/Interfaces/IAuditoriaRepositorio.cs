using System;
using System.Collections.Generic;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.DL.Repository
{
    public interface IAuditoriaRepositorio
    {

        /// <summary>
        /// Devuelve la lsita del Log Cerbero
        /// </summary>
        /// <returns>lista de Evento</returns>
        List<Evento> ConsultarLog(string CodSistema, string Usuario, string FechaInicio, string FechaFin);
    }
}
