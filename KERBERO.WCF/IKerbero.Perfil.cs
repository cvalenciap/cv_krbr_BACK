using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IKerbero" en el código y en el archivo de configuración a la vez.
    [ServiceKnownType(typeof(Perfil))]
    [ServiceKnownType(typeof(List<Perfil>))]
    public partial interface IKerbero
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "perfil?sistema={Sistema}&nombre={Nombre}&estado={Estado}")]
        RespuestaOperacionServicio BuscarPerfiles(string Sistema, string Nombre, string Estado);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "perfil/{Id}")]
        RespuestaOperacionServicio ObtenerPerfil(string Id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "perfil")]
        RespuestaOperacionServicio CrearPerfil(Perfil perfil);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "perfil/{Id}")]
        RespuestaOperacionServicio ModificarPerfil(string Id, Perfil perfil);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "perfil/{Id}/{Estado}/{idUsuario}")]
        RespuestaOperacionServicio CambiarEstadoPerfil(string idUsuario, string Id, string Estado);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "perfilvar/{Estado}/{idUsuario}")]
        RespuestaOperacionServicio CambiarEstadosPerfiles(OperacionMultiplePerfil objOperacion, string Estado, string idUsuario);
    }
}
