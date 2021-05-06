using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class Estado
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Valor { get; set; }
    }
}
