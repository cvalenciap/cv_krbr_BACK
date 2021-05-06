///---------------------------------------------------------------------------------------------------------------------------------------------------------------
///   Namespace:        pide_api
///   Objeto:           Constants
///   Descripcion:      Clase que contiene parámetros constantes
///   Autor:            
///---------------------------------------------------------------------------------------------------------------------------------------------------------------
///   Historia de modificaciones:
///   Requerimiento:         Autor:            Fecha:          Descripcion:
///   REQ2086                cvalenciap		   10/01/2018      Agregación de parámetros constantes utilizados por el requerimiento
///---------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KERBERO.Util
{
    public class Constants
    {
        //public static String SOAP_COD_USER = "N00003";
        //public static String SOAP_COD_TRANSAC = "5";
        //public static String SOAP_COD_ENTIDAD = "03";

        public static String CERBERO_SYSTEM_CODE = "KERBERO";
        public static String CERBERO_VERSION = "1.0";
        public static int CERBERO_TYPE_INFORMATION = 0;
        
        public static int RESPUESTA_KERBERO_OK = 1;
        public static int RESPUESTA_KERBERO_ERROR = 0;
        public static int RESPUESTA_CAPTCHA_ERROR = -2;
    }
}