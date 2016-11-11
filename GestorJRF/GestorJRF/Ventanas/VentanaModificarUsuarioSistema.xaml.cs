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
    public partial class VentanaModificarUsuarioSistema : Window
    {
        public VentanaModificarUsuarioSistema()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }
        
        private void bModificarUsuario_Click(object sender, RoutedEventArgs e)
        {
            UsuarioSistema usuario = new UsuarioSistema(tUsuario.Text, tPassAntigua.Text, "");
            var salida = UsuarioSistemaCRUD.modificacionAutomaticaContraseña(usuario);
            if (!salida.Equals(""))
            {
                usuario = new UsuarioSistema(tUsuario.Text, tPassNueva.Text, tEmail.Text);
                if (UsuarioSistemaCRUD.modificacionUsuarioSistema(usuario) == 1)
                {
                    MessageBox.Show("Se ha modificado correctamente los datos del usuario.", "Nueva alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                    
            }
        }
    }
}
