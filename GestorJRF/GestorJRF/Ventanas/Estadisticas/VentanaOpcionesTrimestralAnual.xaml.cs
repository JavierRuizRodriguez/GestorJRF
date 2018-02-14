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
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.ventanaPadre = ventanaPadre;
            this.tipo = tipo;
            this.tAño.Text = DateTime.Now.Year.ToString();
            if (tipo.Contains("Anual"))
            {
                this.lTrimestre.Visibility = Visibility.Hidden;
                this.cTrimestre.Visibility = Visibility.Hidden;
                this.lAño.Margin = new Thickness(77.0, 11.0, 0.0, 0.0);
                this.tAño.Margin = new Thickness(114.0, 15.0, 0.0, 0.0);
            }
        }

        private void bGenerarEstadistica_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVerificacion.validadorNumeroEntero(this.tAño.Text))
            {
                int año = Convert.ToInt32(this.tAño.Text);
                DateTime inicio;
                DateTime final;
                if (!this.tipo.Contains("Anual"))
                {
                    switch (this.cTrimestre.SelectedIndex)
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
                this.ventanaPadre.opciones = new BusquedaEstadisticas(inicio, final, this.tipo);
                if (!this.tipo.Contains("comparativa"))
                {
                    this.ventanaPadre.generarGraficaLineal();
                }
                else
                {
                    this.ventanaPadre.generarGraficoBarras();
                }
                base.Close();
            }
        }
    }
}
