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

namespace GestorJRF.Ventanas.Facturas
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionFacturacion.xaml
    /// </summary>
    public partial class VentanaSeleccionFacturacion : Window
    {
        private bool xBotonPulsado;
        public VentanaSeleccionFacturacion()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            xBotonPulsado = true;
        }

        private void bFacturaResumenes_Click(object sender, RoutedEventArgs e)
        {
            xBotonPulsado = false;
            new VentanaFacturaResumenes().Show();
            Close();
        }

        private void bFacturaIva_Click(object sender, RoutedEventArgs e)
        {
            xBotonPulsado = false;
            new VentanaFacturaIva().Show();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (xBotonPulsado)
                new VentanaMenuPrincipal().Show();
        }
    }
}
