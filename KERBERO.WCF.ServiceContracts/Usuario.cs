using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class Usuario
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Login { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Contrasena { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ApePaterno { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ApeMaterno { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Nombre { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string NombreCompleto { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Email { get; set; }
        [DataMember]
        public int Estado { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int NroIntento { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public String FechaCaducidad { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int User { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string HorarioAcceso { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FechaRegistro { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int ReqValidacion { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string LlavePC { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int PermisoLlavePC { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Permiso> Permisos { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Perfil> Perfiles { get; set; }
    }

    public class OperacionMultipleUsuario
    {
        [DataMember]
        public List<int> Id { get; set; }

        [DataMember]
        public int Estado { get; set; }
    }
}
