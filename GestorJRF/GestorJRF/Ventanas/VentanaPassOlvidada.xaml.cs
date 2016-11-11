using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaPassOlvidada.xaml
    /// </summary>
    public partial class VentanaPassOlvidada : Window
    {
        public VentanaPassOlvidada()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void bGenerarNuevaContraseña_Click(object sender, RoutedEventArgs e)
        {
            var nueva = cadenaAleatoria(20);
            UsuarioSistema usuario = new UsuarioSistema(tUsuario.Text, nueva, "");
            var salida = UsuarioSistemaCRUD.modificacionAutomaticaContraseña(usuario);
            if (!salida.Equals(""))
            {
                usuario.email = salida;
                if(enviarEmailInformativo(usuario, nueva))
                {
                    MessageBox.Show("Se ha enviado la contraseña al email del usuario. Compruebela y modifíquela.", "Nueva alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
        }

        private bool enviarEmailInformativo(UsuarioSistema usuario, string nueva)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("gestorjrf.info@gmail.com");
            msg.To.Add(usuario.email);
            msg.Subject = "Nueva contraseña";
            msg.Body = "Se ha generado una nueva contraseña: " + nueva + "\n" + "Por favor, modifique la contraseña.";
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("gestorjrf.info@gmail.com", "transportesRuizFraile");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                enviarEmailInformativo(usuario, nueva);
                return true;
            }
            finally
            {
                msg.Dispose();
            }
        }

        private string cadenaAleatoria(int longitud)
        {
            Random random = new Random();
            const string caracteres = "abcdefghijklmnñopqrstuvwyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.¿?%$";
            return new string(Enumerable.Repeat(caracteres, longitud).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
