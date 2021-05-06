using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    [ServiceKnownType(typeof(Sistema))]
    [ServiceKnownType(typeof(List<Sistema>))]
    public partial interface IKerbero
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "sistema?nombre={Nombre}&estado={Estado}")]
        RespuestaOperacionServicio BuscarSistemas(string Nombre, string Estado);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "sistema/{Id}")]
        RespuestaOperacionServicio ObtenerSistema(string Id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "sistema")]
        RespuestaOperacionServicio CrearSistema(Sistema sistema);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "sistema/{Id}")]
        RespuestaOperacionServicio ModificarSistema(string Id, Sistema sistema);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "sistema/{Id}/{Estado}/{idUsuario}")]
        RespuestaOperacionServicio CambiarEstadoSistema(string idUsuario, string Id, string Estado);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "sistemavar/{Estado}/{idUsuario}")]
        RespuestaOperacionServicio CambiarEstadosSistemas(OperacionMultipleSistema objOperacion, string Estado, string idUsuario);
    }
}
