using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.Login
{
    /// <summary>
    /// Lógica de interacción para VentanaPassOlvidada.xaml
    /// </summary>
    public partial class VentanaPassOlvidada : Window
    {
        public VentanaPassOlvidada()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void bGenerarNuevaContraseña_Click(object sender, RoutedEventArgs e)
        {
            string nueva = this.cadenaAleatoria(20);
            UsuarioSistema usuario = new UsuarioSistema(this.tUser.Text, nueva, "");
            string salida = UsuarioSistemaCRUD.modificacionAutomaticaContraseña(usuario);
            if (!salida.Equals(""))
            {
                usuario.email = salida;
                if (this.enviarEmailInformativo(usuario, nueva))
                {
                    MessageBox.Show("Se ha enviado la contraseña al email del usuario. Compruebela y modifíquela.", "Nueva alerta", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    base.Close();
                }
            }
        }

        private bool enviarEmailInformativo(UsuarioSistema usuario, string nueva)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("gestorjrf.info@gmail.com");
            msg.To.Add(usuario.email);
            msg.Subject = "Nueva contraseña";
            msg.Body = "Se ha generado una nueva contraseña: " + nueva + "\nPor favor, modifique la contraseña.";
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
                this.enviarEmailInformativo(usuario, nueva);
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
            return new string((from s in Enumerable.Repeat("abcdefghijklmnñopqrstuvwyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.¿?%$", longitud)
                               select s[random.Next(s.Length)]).ToArray());
        }

        public void Connect(int connectionId, object target)
        {
            throw new NotImplementedException();
        }
    }
}
