using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Net;

namespace CERBERO.Cliente.Contratos
{
    [DataContract]
    public class RespuestaCerbero
    {
        //[DataMember]
        //public HttpStatusCode CodigoHttp { get; set; }

        [DataMember]
        public RestDataResponse response { get; set; }
        //[DataMember]
        //public int CodigoHttp { get; set; }

        //[DataMember]
        //public int CodigoRespuesta { get; set; }

        //[DataMember]
        //public string Mensaje { get; set; }

        //[DataMember]
        //public string MensajeInterno { get; set; }

        //[DataMember]
        //public Usuario ObjUsuario { get; set; }

        //[DataMember]
        //public Sistema ObjSistema { get; set; }

        //[DataMember]
        //public HttpResponseMessage Respuesta { get; set; }
    }

    [DataContract]
    public class RestDataResponse
    {
        //[DataMember(Name = "Usuario")]
        //public string NombreUsuario { get; set; }

        //[DataMember(Name = "Password")]
        //public string Password { get; set; }

        //[DataMember]
        //public int TipoUsuario { get; set; }

        //[DataMember]
        //public string IdSistema { get; set; }

        //[DataMember]
        //public string Opciones { get; set; }

        [DataMember]
        public static int STATUS_OK = 1;
        [DataMember]
        public static int STATUS_ERROR = 0;
        [DataMember]
        public int idUsuario { get; set; }
        [DataMember]
        public string usuario { get; set; }
        [DataMember]
        public string perfil { get; set; }
        [DataMember]
        public string tokenString { get; set; }
        [DataMember]
        public List<string> opciones { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public String message { get; set; }

        public RestDataResponse(string usuario, string tokenString, List<string> opciones /*object data*/, int status, String mensaje)
        {
            //this.data = data;
            this.usuario = usuario;
            this.tokenString = tokenString;
            this.opciones = opciones;
            this.status = status;
            this.message = mensaje;
        }

    }

    //[DataContract]
    //public class Sistema
    //{
    //    [DataMember(Name = "Sistema")]
    //    public string IdSistema { get; set; }

    //    [DataMember]
    //    public string Version { get; set; }
    //}
}
