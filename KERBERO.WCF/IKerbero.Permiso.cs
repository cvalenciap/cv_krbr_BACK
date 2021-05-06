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
    [ServiceKnownType(typeof(Permiso))]
    [ServiceKnownType(typeof(List<Permiso>))]
    public partial interface IKerbero
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "permiso?sistema={Sistema}&codigo={Codigo}&nombre={Nombre}&estado={Estado}")]
        RespuestaOperacionServicio BuscarPermisos(string Sistema, string Codigo, string Nombre, string Estado);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "permiso/{Id}")]
        RespuestaOperacionServicio ObtenerPermiso(string Id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "permiso")]
        RespuestaOperacionServicio CrearPermiso(Permiso permiso);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "permiso/{Id}")]
        RespuestaOperacionServicio ModificarPermiso(string Id, Permiso permiso);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "permiso/{Id}/{Estado}/{idUsuario}")]
        RespuestaOperacionServicio CambiarEstadoPermiso(string idUsuario, string Id, string Estado);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "permisovar/{Estado}/{idUsuario}")]
        RespuestaOperacionServicio CambiarEstadosPermisos(OperacionMultiplePermiso objOperacion, string Estado, string idUsuario);
    }
}
