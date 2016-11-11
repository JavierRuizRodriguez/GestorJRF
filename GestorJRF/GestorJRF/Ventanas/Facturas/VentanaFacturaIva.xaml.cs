using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.Utilidades;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.Facturas
{
    /// <summary>
    /// Lógica de interacción para VentanaFactura.xaml
    /// </summary>
    public partial class VentanaFacturaIva : Window
    {
        private bool pulsadoBotonX;

        public VentanaFacturaIva()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            tAño.Text = Convert.ToString(DateTime.Now.Year);
            pulsadoBotonX = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (pulsadoBotonX)
                new VentanaMenuPrincipal().Show();
        }

        private void bGenerarFactura_Click(object sender, RoutedEventArgs e)
        {
            DateTime inicio;
            DateTime final;

            if (UtilidadesVerificacion.validadorNumeroEntero(tAño.Text))
            {
                int año = Convert.ToInt32(tAño.Text);
                switch (cTrimestre.SelectedIndex)
                {
                    case 0:
                        inicio = new DateTime(año, 1, 1);
                        final = new DateTime(año, 3, 31);
                        break;
                    case 1:
                        inicio = new DateTime(año, 4, 1);
                        final = new DateTime(año, 6, 30);
                        break;
                    case 2:
                        inicio = new DateTime(año, 7, 1);
                        final = new DateTime(año, 9, 30);
                        break;
                    case 3:
                        inicio = new DateTime(año, 10, 1);
                        final = new DateTime(año, 12, 31);
                        break;
                    default:
                        inicio = new DateTime(año, 1, 1);
                        final = new DateTime(año, 3, 31);
                        break;
                }

                BusquedaFactura busqueda = new BusquedaFactura(inicio, final, "iva");
                new VentanaImpresionFactura(busqueda).Show();
                pulsadoBotonX = false;
                Close();
            }         
        }
    }
}
