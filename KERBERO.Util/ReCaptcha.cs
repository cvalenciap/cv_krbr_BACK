///-------------------------------------------------------------------------------------
///   Namespace:        KERBERO.Util
///   Objeto:           ReCaptcha
///   Descripcion:      Objeto para autenticar Google ReCAPTCHA
///   Autor:            Daniel Salas
///-------------------------------------------------------------------------------------
///   Historia de modificaciones:
///   Requerimiento:    Autor:            Fecha:        Descripcion:
///-------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;

namespace KERBERO.Util
{
    public class ReCaptcha
    {
        public static bool Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient();
            string PrivateKey = ConfigurationManager.AppSettings["ReCAPTCHAPrivateKey"].ToString();
            string EndpointUrl = ConfigurationManager.AppSettings["ReCAPTCHAEndpoint"].ToString();
            var GoogleReply = client.DownloadString(string.Format("{0}?secret={1}&response={2}", EndpointUrl, PrivateKey, EncodedResponse));
            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptcha>(GoogleReply);
            return captchaResponse.Success.ToLower() == "true";
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }

        private List<string> m_ErrorCodes;
    }
}