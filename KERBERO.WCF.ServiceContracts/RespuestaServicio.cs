using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class RespuestaOperacionServicio
    {
        [DataMember]
        public int Resultado { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Error { get; set; }

        [DataMember]
        public Object data { get; set; }
    }
}
