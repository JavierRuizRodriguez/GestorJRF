using GestorJRF.Utilidades;
using GestorJRF.Ventanas.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para VentanaOperacionesTerceros.xaml
    /// </summary>
    public partial class VentanaOperacionesTerceros : Window
    {
        private bool xBotonPulsado;
        private const string tipo = "operacionesTerceros";

        public VentanaOperacionesTerceros()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.xBotonPulsado = true;
            this.tAño.Content = DateTime.Now.Year.ToString();
        }

        private void bGenerarFactura_Click(object sender, RoutedEventArgs e)
        {
            this.xBotonPulsado = false;
            if (!this.cbTipo.Text.Equals(""))
            {
                new VentanaImpresionFactura(tipo, this.cbTipo.Text.Equals("Facturas emitidas") ? "facturas" : "iva", int.Parse(tAño.Content.ToString()));
                base.Close();
            }
            else
                MessageBox.Show("Debe seleccionar un tipo de factura", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.xBotonPulsado)
            {
                new VentanaMenuPrincipal().Show();
            }
        }

        private void bArriba_Click(object sender, RoutedEventArgs e)
        {
            tAño.Content = (int.Parse(tAño.Content.ToString()) + 1).ToString();
        }

        private void bAbajo_Click(object sender, RoutedEventArgs e)
        {
            tAño.Content = (int.Parse(tAño.Content.ToString()) - 1).ToString();
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
