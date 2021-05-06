using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CERBERO.Cliente.Contratos
{
    [DataContract]
    public class RespuestaAutenticarUsuario
    {
        [DataMember]
        public int CodigoHttp { get; set; }

        [DataMember]
        public int CodigoRespuesta { get; set; }

        [DataMember]
        public string Mensaje { get; set; }

        [DataMember]
        public Usuario Objeto { get; set; }
    }

    [DataContract]
    public class Usuario
    {
        [DataMember(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public int TipoUsuario { get; set; }

        [DataMember]
        public string IdSistema { get; set; }

        [DataMember]
        public string Opciones { get; set; }
    }
}
