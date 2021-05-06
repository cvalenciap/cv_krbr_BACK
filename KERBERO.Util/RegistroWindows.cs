///-------------------------------------------------------------------------------------
///   Namespace:        CERBERO.Util
///   Objeto:           RegistroWindows
///   Descripcion:      Métodos para acceder a valores del registro de Windows
///   Autor:            Andrea Sánchez
///-------------------------------------------------------------------------------------
///   Historia de modificaciones:
///   Requerimiento:    Autor:            Fecha:        Descripcion:
///-------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace KERBERO.Util
{
    /// <summary>
    /// Métodos para acceder a valores del registro de Windows
    /// </summary>
    public abstract class RegistroWindows
    {
        /// <summary>
        /// Obtiene un valor del registro de Windows (32 bits)
        /// </summary>
        /// <param name="strRuta">ruta donde se encuentra la clave</param>
        /// <param name="strClave">nombre de la clave</param>
        /// <returns>valor almacenado en el registro</returns>
        public static string ObtenerCadenaRegistro(string strRuta, string strClave)
        {
           RegistryKey RegistroLocal
               = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                          Environment.Is64BitOperatingSystem
                                              ? RegistryView.Registry64
                                              : RegistryView.Registry32);
           RegistryKey registro = string.IsNullOrEmpty(strRuta) 
               ? RegistroLocal 
               : RegistroLocal.OpenSubKey(strRuta);
           return registro.GetValue(strClave).ToString();
        }
    }
}
