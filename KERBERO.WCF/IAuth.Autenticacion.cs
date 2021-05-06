using KERBERO.WCF.ServiceContracts;
using System.ServiceModel;
using System.ServiceModel.Web;
using CERBERO.Cliente.Contratos;

namespace KERBERO.WCF
{
    [ServiceKnownType(typeof(RespuestaCerbero))]
    public partial interface IKerbero
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "login")]
        RespuestaOperacionServicio AutenticarUsuario(UsuarioAuth data);       
    }
}
