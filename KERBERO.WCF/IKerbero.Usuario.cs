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
    [ServiceKnownType(typeof(Usuario))]
    [ServiceKnownType(typeof(List<Usuario>))]
    public partial interface IKerbero
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "usuario?apePaterno={ApePaterno}&apeMaterno={ApeMaterno}&nombre={Nombre}&login={Login}&estado={Estado}")]
        RespuestaOperacionServicio BuscarUsuarios(string ApePaterno, string ApeMaterno, string Nombre, string Login, string Estado);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "usuario/{Id}")]
        RespuestaOperacionServicio ObtenerUsuario(string Id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "usuario")]
        RespuestaOperacionServicio CrearUsuario(Usuario usuario);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "usuario/{Id}")]
        RespuestaOperacionServicio ModificarUsuario(string Id, Usuario usuario);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "usuario/{Id}/{Estado}/{idUsuario}")]
        RespuestaOperacionServicio CambiarEstadoUsuario(string idUsuario, string Id, string Estado);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "usuariovar/{Estado}/{idUsuario}")]
        RespuestaOperacionServicio CambiarEstadosUsuarios(OperacionMultipleUsuario objOperacion, string Estado, string idUsuario);
    }
}
