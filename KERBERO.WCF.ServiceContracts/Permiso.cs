using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class Permiso
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Sistema Sistema { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Codigo { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Nombre { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Descripcion { get; set; }
        [DataMember]
        public int Estado { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int Usuario { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Valor { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public String FechaCreacion { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Perfil> Perfiles { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Usuario> Usuarios { get; set; }
    }

    public class OperacionMultiplePermiso
    {
        [DataMember]
        public List<int> Id { get; set; }

        [DataMember]
        public int Estado { get; set; }
    }
}
