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
    [ServiceKnownType(typeof(Parametro))]
    [ServiceKnownType(typeof(List<Parametro>))]
    public partial interface IKerbero
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "parametro?paramPadre={ParamPadre}&estado={Estado}")]
        RespuestaOperacionServicio BuscarParametros(string ParamPadre, string Estado);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "parametro/{Id}")]
        RespuestaOperacionServicio ObtenerParametro(string Id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "parametroValor/{Id}")]
        RespuestaOperacionServicio ValorParametro(string Id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "parametro")]
        RespuestaOperacionServicio CrearParametro(Parametro parametro);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "parametro/{Id}")]
        RespuestaOperacionServicio ModificarParametro(string Id, Parametro parametro);
    }
}
