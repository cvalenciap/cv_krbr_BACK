using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class VersionSistema
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Version { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Ticket { get; set; }
        [DataMember]
        public int Estado { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public String CrearFecha { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public String ActualizarFecha { get; set; }
    }
}
