///---------------------------------------------------------------------------------------------------------------------------------------------------------------
///   Namespace:        pide_api
///   Objeto:           ErrorMessagesUtil
///   Descripcion:      Clase que contiene las respuestas sobre errores en la ejecución del aplicativo
///   Autor:            
///---------------------------------------------------------------------------------------------------------------------------------------------------------------
///   Historia de modificaciones:
///   Requerimiento:         Autor:            Fecha:          Descripcion:
///   REQ2086                cvalenciap		   10/01/2018      Agregación de parámetros de error
///---------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace KERBERO.Util
{
    public class ErrorMessagesUtil
    {
        public static string OBTUVO_RESULTADO_BUSQUEDA = "";
        public static string NO_OBTUVO_RESULTADO_BUSQUEDA = "No se encontraron resultados";

        public static string SERVICIO_HABILITADO = "El servicio se encuentra habilitado";
        public static string SERVICIO_INHABILITADO = "El servicio no se encuentra habilitado por ahora";

        //REQ2086 - INICIO
        public static string CREDENCIALES_INCONSISTENTES = "La credencial antigua ingresada no es la correcta";
        public static string USUARIO_NO_REGISTRADO = "El usuario no se encuentra registrado en Base de Datos";
        public static string NO_OBTUVO_RESULTADO_ACTUALIZACION = "No se devolvieron resultados de la actualización";
        public static string NO_ACTUALIZO_BD = "No se pudo actualizar las credenciales en Base de Datos(VERIFICAR), CLAVE DE SERVICIO CAMBIADA";
        //REQ2086 - FIN

        public static string ERROR_CONEXION_BD = "Error de conexión a la Base de Datos";

        public static string ObtenerMensajeDNI(int codigo)
        {
            string baseError = "Error RENIEC: ";
            switch (codigo) {
                case -1:
                    return baseError + "Error en el Servidor";
                case -2:
                    return baseError + "Sesión Expirada";
                case -3:
                    return baseError + "Excedió el máximo nro de consultas por minuto";
                case -4:
                    return baseError + "Código de operación no existe";
                case -5:
                    return baseError + "Usuario Invalido";
                case -6:
                    return baseError + "No se puede acceder al servicio en esta fecha";
                case -7:
                    return baseError + "Formato de DNI no valido";
                case -8:
                    return baseError + "No existe DNI en base de datos";
                case -9:
                    return baseError + "Data incompleta en documento XML";
                case -10:
                    return baseError + "No es un documento XML";
            }

            return baseError + "Error no especifico en documentacion";
        }

        public static int ErrorConexionServicio(Exception ex)
        {
            switch (ex.GetType().ToString())
            {
                case "System.Net.WebException":
                    var status = ((WebException)ex).Status;
                    if(status == WebExceptionStatus.Timeout)
                    {
                        return 2;
                    }
                    else if (status == WebExceptionStatus.ProtocolError)
                    {
                        return 3;
                    }
                    else
                    {
                        Debug.Print(status.ToString());
                    }
                    return 1;
            }

            return 0;
        }

        public static HttpError userNoExits()
        {
            var message = string.Format("Usuario no registrado.");
            return new HttpError(message);
        }

        public static HttpError incorrectPassword()
        {
            var message = string.Format("Credenciales incorrectas.");
            return new HttpError(message);
        }

        public static HttpError formatoIncorrecto()
        {
            var message = string.Format("Parametros no enviados correctamente.");
            return new HttpError(message);
        }

        public static HttpError error()
        {
            var message = string.Format("Error Interno.");
            return new HttpError(message);
        }

        public static HttpError notFoundDni()
        {
            var message = string.Format("No se ha encontrado el DNI consultado.");
            return new HttpError(message);
        }

        public static HttpError notFoundRuc()
        {
            var message = string.Format("No se ha encontrado el RUC consultado.");
            return new HttpError(message);
        }

        public static HttpError notHaveResults()
        {
            var message = string.Format("No se ha encontrado resultados.");
            return new HttpError(message);
        }

        public static HttpError cannotGetInfo()
        {
            var message = string.Format("No se pudo obtener información del webservices.");
            return new HttpError(message);
        }

        public static HttpError timeoutRequest()
        {
            var message = string.Format("El tiempo de respuesta ha llegado a su límite.");
            return new HttpError(message);
        }

        public static HttpError cannotConnectCerbero()
        {
            var message = string.Format("No se ha podido conectar con Cerbero.");
            return new HttpError(message);
        }

        public static HttpError cannotLoginCerbero()
        {
            var message = string.Format("No se ha podido autenticar al usuario.");
            return new HttpError(message);
        }
    }
}