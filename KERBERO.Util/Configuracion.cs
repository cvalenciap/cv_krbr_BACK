///-------------------------------------------------------------------------------------
///   Namespace:        CERBERO.Servicios
///   Objeto:           Configuracion
///   Descripcion:      Obtiene los parámetros de configuración establecidos en Web.config
///   Autor:            Daniel Salas
///-------------------------------------------------------------------------------------
///   Historia de modificaciones:
///   Requerimiento:    Autor:            Fecha:        Descripcion:
///-------------------------------------------------------------------------------------
using System;
using System.Configuration;
using System.Collections.Generic;

namespace KERBERO.BL.Components
{
    /// <summary>
    /// Obtiene los parámetros de configuración establecidos en Web.config
    /// </summary>
    public static class Configuracion
    {
        /// <summary>
        /// Ruta del registro de windows donde se encuentra la cadena de conexión
        /// </summary>
        public static string RutaKerbero
        {
            get
            {
                return ConfigurationManager.AppSettings["RutaKerbero"].ToString();
            }
        }

        /// <summary>
        /// Nombre de la clave del registro de windows donde se encuentra la cadena de conexión
        /// </summary>
        public static string ClaveConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ClaveConnectionString"].ToString();
            }
        }
    }
}
