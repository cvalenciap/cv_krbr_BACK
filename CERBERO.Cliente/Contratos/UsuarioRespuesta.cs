using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CERBERO.Cliente.Contratos
{   
    public class UsuarioRespuesta
    {
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public String username { get; set; }
        /// <summary>
        /// Token de usuario autentificado
        /// </summary>
        public String authToken { get; set; }
        /// <summary>
        /// Opciones disponibles de usuario
        /// </summary>
        public List<String> opciones { get; set; }

        public UsuarioRespuesta(String username, String authToken, List<String> opciones)
        {
            this.username = username;
            this.authToken = authToken;
            this.opciones = opciones;
        }
    }
}