using GestorJRF.Utilidades;
using System.Windows;

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaOpcionesModificacionUsuario.xaml
    /// </summary>
    public partial class VentanaOpcionesModificacionUsuario : Window
    {
        public VentanaOpcionesModificacionUsuario()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void bModificarDatos_Click(object sender, RoutedEventArgs e)
        {
            new VentanaModificarUsuarioSistema().Show();
            Close();
        }

        private void bContraseñaOlvidada_Click(object sender, RoutedEventArgs e)
        {
            new VentanaPassOlvidada().Show();
            Close();
        }
    }
}
