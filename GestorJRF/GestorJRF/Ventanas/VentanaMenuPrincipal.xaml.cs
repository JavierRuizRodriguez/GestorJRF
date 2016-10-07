using GestorJRF.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaMenuPrincipal.xaml
    /// </summary>
    public partial class VentanaMenuPrincipal : Window
    {
        public VentanaMenuPrincipal()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
        }

        private void bGestionResumenes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void bGestionDatosGenerales_Click(object sender, RoutedEventArgs e)
        {
            VentanaMenuGestionDatos vGestionDatosGenerales = new VentanaMenuGestionDatos();
            Close();
            vGestionDatosGenerales.Show();
        }
    }
}
