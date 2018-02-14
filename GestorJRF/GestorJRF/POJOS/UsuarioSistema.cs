using System.Security.Cryptography;
using System.Text;

namespace GestorJRF.POJOS
{
    public class UsuarioSistema
    {
        public string usuario
        {
            get;
            set;
        }

        public string contraseña
        {
            get;
            set;
        }

        public string email
        {
            get;
            set;
        }

        public UsuarioSistema()
        {
        }

        public UsuarioSistema(string usuario, string contraseña, string email)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(contraseña);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                this.contraseña = sb.ToString();
            }
            this.usuario = usuario;
            this.email = email;
        }
    }
}
