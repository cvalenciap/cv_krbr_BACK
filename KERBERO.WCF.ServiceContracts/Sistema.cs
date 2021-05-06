using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KERBERO.WCF.ServiceContracts
{
    [DataContract]
    public class Sistema
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Codigo { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Nombre { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Descripcion { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Version { get; set; }
        [DataMember]
        public int Estado { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FechaRegistro { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string HorarioAcceso { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int Usuario { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Permiso> Permisos { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Perfil> Perfiles { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<VersionSistema> Versiones { get; set; }
    }

    public class OperacionMultipleSistema
    {
        [DataMember]
        public List<int> Id { get; set; }

        [DataMember]
        public int Estado { get; set; }
    }
}
