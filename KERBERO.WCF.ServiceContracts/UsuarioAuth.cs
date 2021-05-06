using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class UsuarioAuth
    {
        [DataMember]
        public string user { get; set; }
        [DataMember]
        public string pass { get; set; }
        [DataMember]
        public string captcha { get; set; }

    }
}
