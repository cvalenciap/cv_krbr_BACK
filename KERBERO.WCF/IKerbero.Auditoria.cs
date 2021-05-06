using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using KERBERO.WCF.ServiceContracts;

namespace KERBERO.WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IKerbero" en el código y en el archivo de configuración a la vez.
    [ServiceKnownType(typeof(Evento))]
    [ServiceKnownType(typeof(List<Evento>))]
    public partial interface IKerbero
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "log?sistema={Sistema}&usuario={Usuario}&fechaInicio={FechaInicio}&fechaFin={FechaFin}")]
        RespuestaOperacionServicio BuscarAuditoria(string Sistema, string Usuario, string FechaInicio, string FechaFin);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "log?sistema={Sistema}&usuario={Usuario}&fechaInicio={FechaInicio}&fechaFin={FechaFin}")]
        RespuestaOperacionServicio ExportarExcel(string Sistema, string Usuario, string FechaInicio, string FechaFin);
    }
}
