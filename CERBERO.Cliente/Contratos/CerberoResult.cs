using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CERBERO.Cliente.Contratos
{
    public class CerberoResult
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public int respuesta { get; set; }
        public string opciones { get; set; }

        public CerberoResult(int codigo, string mensaje, int respuesta, string opciones)
        {
            this.codigo = codigo;
            this.mensaje = mensaje;
            this.respuesta = respuesta;
            this.opciones = opciones;
        }
    }
}
