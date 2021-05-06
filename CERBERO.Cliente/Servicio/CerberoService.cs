using CERBERO.Cliente.Contratos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CERBERO.Cliente.Servicio
{
    public class CerberoService
    {
        private const int RESPUESTA_OK = 1;

        public static CerberoResult ValidarVersion(string CodigoSistema, string Version, int TipoInformacion)
        {
            RespuestaValidarVersion respuesta = ClienteServicio.ValidarVersion(CodigoSistema, Version, TipoInformacion);
            if (respuesta.CodigoRespuesta == RESPUESTA_OK) {
                respuesta.CodigoHttp = 1;
            }
            else
            {
                respuesta.CodigoHttp = 0;
            }
            return new CerberoResult(respuesta.CodigoRespuesta, respuesta.Mensaje, respuesta.CodigoHttp, "");
        }

        public static CerberoResult AutenticarUsuario(string CodigoSistema, string Usuario, string Password, int TipoUsuario)
        {
            RespuestaAutenticarUsuario respuesta = ClienteServicio.AutenticarUsuario(CodigoSistema, Usuario, Password, TipoUsuario);
            if (respuesta.CodigoRespuesta == RESPUESTA_OK)
            {
                respuesta.CodigoHttp = 1;
            }
            else
            {
                respuesta.CodigoHttp = 0;
                respuesta.Objeto = new Usuario();
                respuesta.Objeto.Opciones = "";
            }
            return new CerberoResult(respuesta.CodigoRespuesta, respuesta.Mensaje, respuesta.CodigoHttp, respuesta.Objeto.Opciones);
        }
    }
}
