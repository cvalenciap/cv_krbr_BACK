using System;
using System.Linq;
using System.Text;
using KERBERO.WCF.ServiceContracts;
using KERBERO.Util;
using CERBERO.Cliente.Contratos;
using CERBERO.Cliente.Servicio;
using KERBERO.BL.Components;
using System.Collections.Generic;

namespace KERBERO.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Autenticacion" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Autenticacion.svc or Autenticacion.svc.cs at the Solution Explorer and start debugging.
    public partial class Kerbero : IKerbero
    {   
        public RespuestaOperacionServicio AutenticarUsuario(UsuarioAuth data)
        {
            RespuestaOperacionServicio rpta = new RespuestaOperacionServicio();
            RespuestaCerbero respuesta = new RespuestaCerbero();
            try
            {
                string user = data.user;
                string pass = data.pass;
                string captcha = data.captcha;

                bool bValidacionCaptcha = ReCaptcha.Validate(captcha);
                if (!bValidacionCaptcha)
                {
                    rpta.Resultado = Constants.RESPUESTA_CAPTCHA_ERROR;
                    rpta.Error = "La validación CAPTCHA no ha sido correcta.";
                    return rpta;
                }

                CerberoResult resultValidate = CerberoService.ValidarVersion(Constants.CERBERO_SYSTEM_CODE, Constants.CERBERO_VERSION, Constants.CERBERO_TYPE_INFORMATION);

                if (resultValidate.respuesta == 1)
                {
                    CerberoResult resultAuntenticar = CerberoService.AutenticarUsuario(Constants.CERBERO_SYSTEM_CODE, user, pass, 0);

                    if (resultAuntenticar.respuesta == 1)
                    {
                        string tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}{1}{2}", user, DateTime.Now.Millisecond, DateTime.Now.ToShortTimeString())));

                        List<string> opciones = resultAuntenticar.opciones.Split('|').ToList();
                        UsuarioRespuesta usuario = new UsuarioRespuesta(user, tokenString, opciones);

                        RestDataResponse response = new RestDataResponse(user, tokenString, opciones, RestDataResponse.STATUS_OK, "Usuario logueado correctamente");
                        respuesta.response = response;

                        Usuarios ObjUsuario = new Usuarios();
                        string IDUsuarioPerfil = ObjUsuario.obtenerIdUsuario(respuesta.response.usuario.ToUpper());
                        respuesta.response.idUsuario = Convert.ToInt32(IDUsuarioPerfil.Split('|')[0]);
                        respuesta.response.perfil = IDUsuarioPerfil.Split('|')[1];

                        rpta.Resultado = Constants.RESPUESTA_KERBERO_OK;
                        rpta.data = respuesta;
                    }
                    else
                    {
                        throw new Exception(resultAuntenticar.mensaje);
                    }
                }
                else
                {
                    throw new Exception(resultValidate.mensaje);
                }
            }
            catch (Exception ex)
            {
                rpta.Resultado = Constants.RESPUESTA_KERBERO_ERROR;
                rpta.Error = ex.Message;
            }
            return rpta;
        }
    }
}
