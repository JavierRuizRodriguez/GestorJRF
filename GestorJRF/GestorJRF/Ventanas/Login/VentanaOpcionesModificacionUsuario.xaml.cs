using GestorJRF.Utilidades;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.Login
{
    /// <summary>
    /// Lógica de interacción para VentanaOpcionesModificacionUsuario.xaml
    /// </summary>
    public partial class VentanaOpcionesModificacionUsuario : Window
    {
        public VentanaOpcionesModificacionUsuario()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void bModificarDatos_Click(object sender, RoutedEventArgs e)
        {
            new VentanaModificarUsuarioSistema().Show();
            base.Close();
        }

        private void bContraseñaOlvidada_Click(object sender, RoutedEventArgs e)
        {
            new VentanaPassOlvidada().Show();
            base.Close();
        }
    }
}
