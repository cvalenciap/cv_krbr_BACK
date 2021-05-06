using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class Evento
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime FechaCreacion { get; set; }
        [DataMember]
        public string Sistema { get; set; }
        [DataMember]
        public string Usuario { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Origen { get; set; }
    }
}
