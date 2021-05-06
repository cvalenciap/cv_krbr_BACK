using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CERBERO.Cliente.Contratos
{
    [DataContract]
    public class RespuestaValidarVersion
    {
        [DataMember]
        public int CodigoHttp { get; set; }
        [DataMember]
        public int CodigoRespuesta { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
        [DataMember]
        public Sistema Objeto { get; set; }
    }

    [DataContract]
    public class Sistema
    {
        [DataMember(Name = "Sistema")]
        public string IdSistema { get; set; }

        [DataMember]
        public string Version
        { get; set; }
    }
}
