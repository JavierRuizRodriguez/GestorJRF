using GestorJRF.POJOS.Estadisticas;
using GestorJRF.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace GestorJRF.Ventanas.Estadisticas
{
    /// <summary>
    /// Lógica de interacción para VentanaOpciones.xaml
    /// </summary>
    public partial class VentanaOpcionesMensual : Window
    {
        private VentanaEstadisticas ventanaPadre;
        private string tipo;

        public VentanaOpcionesMensual(VentanaEstadisticas ventanaPadre, string tipo)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.ventanaPadre = ventanaPadre;
            this.tipo = tipo;
            this.tAño.Text = DateTime.Now.Year.ToString();
        }

        private void bGenerarEstadistica_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVerificacion.validadorNumeroEntero(this.tAño.Text))
            {
                DateTime inicio = new DateTime(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1, 1);
                DateTime final = new DateTime(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1, DateTime.DaysInMonth(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1));
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
