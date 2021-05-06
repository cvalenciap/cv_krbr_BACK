using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CERBERO.Cliente.Contratos;
using System.Configuration;
using KERBERO.Util;
using System.Net;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace CERBERO.Cliente
{
    public class ClienteServicio
    {
        private static string urlBase = ConfigurationManager.AppSettings["UrlCerberoRest"];
        
        public ClienteServicio()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return (true);
            };
        }

        public static RespuestaValidarVersion ValidarVersion(string CodigoSistema, string Version, int TipoInformacion)
        {
            string urlRuta = "auth/" + CodigoSistema + "/version/" + Version + ";" + TipoInformacion;
            HttpClient cliente = new HttpClient
            {
                BaseAddress = new Uri(urlBase)
            };

            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage respuesta = cliente.GetAsync(urlRuta).Result;

            using (HttpContent contenido = respuesta.Content)
            {
                string strRespuesta = LeerContenido(contenido).Result;
                Debug.Print(strRespuesta);
                RespuestaValidarVersion objRespuesta = JsonConvert.DeserializeObject<RespuestaValidarVersion>(strRespuesta);
                return objRespuesta;
            }
        }

        public static RespuestaAutenticarUsuario AutenticarUsuario(string strCodigoSistema, string strUsuario, string strPassword, int intTipoUsuario)
        {
            string urlRuta = "auth/" + strCodigoSistema;
            HttpClient cliente = new HttpClient
            {
                BaseAddress = new Uri(urlBase)
            };

            var usuario = new Usuario()
            {
                NombreUsuario = strUsuario,
                Password = strPassword,
                TipoUsuario = intTipoUsuario
            };

            string usuarioJson = JsonConvert.SerializeObject(usuario);
            StringContent httpContent = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesta = cliente.PostAsync(urlRuta, httpContent).Result;

            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using (HttpContent contenido = respuesta.Content)
            {
                string strRespuesta = LeerContenido(contenido).Result;
                RespuestaAutenticarUsuario objRespuesta = JsonConvert.DeserializeObject<RespuestaAutenticarUsuario>(strRespuesta);
                return objRespuesta;
            }
        }

        private static async Task<string> LeerContenido(HttpContent contenido)
        {
            return await contenido.ReadAsStringAsync();
        }


    }
}
