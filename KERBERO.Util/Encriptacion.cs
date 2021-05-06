using System;
using System.Security.Cryptography;
using System.Text;

namespace KERBERO.Util
{
    public class Encriptacion
    {
        /// <summary>
        /// Devuelve el hash MD5 de una cadena
        /// </summary>
        /// <param name="strCadena">cadena de texto a convertir</param>
        /// <returns>hash MD5 o Excepción en caso la cadena esté vacía</returns>
        public static string MD5(string strCadena)
        {
            if (strCadena.Equals(string.Empty)) throw new ArgumentOutOfRangeException();

            using (MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(strCadena));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
