using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class Parametro
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Valor { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int IdPadre { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ValorPadre { get; set; }        
        [DataMember]
        public int Estado { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Codigo { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FechaRegistro { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int Usuario { get; set; }
    }

    public class OperacionMultipleParametro
    {
        [DataMember]
        public List<int> Id { get; set; }

        [DataMember]
        public int Estado { get; set; }
    }
}
