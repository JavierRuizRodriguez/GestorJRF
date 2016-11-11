using GestorJRF.POJOS.Estadisticas;
using GestorJRF.Utilidades;
using System;
using System.Windows;
namespace GestorJRF.Ventanas.Estadisticas
{
    /// <summary>
    /// Lógica de interacción para VentanaOpciones.xaml
    /// </summary>
    public partial class VentanaOpcionesTrimestralAnual : Window
    {
        private VentanaEstadisticas ventanaPadre;
        private string tipo;

        public VentanaOpcionesTrimestralAnual(VentanaEstadisticas ventanaPadre, string tipo)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.ventanaPadre = ventanaPadre;
            this.tipo = tipo;
            tAño.Text = DateTime.Now.Year.ToString();

            if (tipo.Contains("Anual"))
            {
                lTrimestre.Visibility = Visibility.Hidden;
                cTrimestre.Visibility = Visibility.Hidden;
                lAño.Margin = new Thickness(77, 11, 0, 0);
                tAño.Margin = new Thickness(114, 15, 0, 0);
            }
        }

        private void bGenerarEstadistica_Click(object sender, RoutedEventArgs e)
        {
            DateTime inicio;
            DateTime final;

            if (UtilidadesVerificacion.validadorNumeroEntero(tAño.Text))
            {
                int año = Convert.ToInt32(tAño.Text);
                if (!tipo.Contains("Anual"))
                {
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
                }
                else
                {
                    inicio = new DateTime(año, 1, 1);
                    final = new DateTime(año, 12, 31);
                }
                ventanaPadre.opciones = new BusquedaEstadisticas(inicio, final, tipo);

                if (!tipo.Contains("comparativa"))
                    ventanaPadre.generarGraficaLineal();
                else
                    ventanaPadre.generarGraficoBarras();

                Close();
            }
        }
    }
}
