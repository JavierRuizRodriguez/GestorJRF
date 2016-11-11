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
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.ventanaPadre = ventanaPadre;
            this.tipo = tipo;
            tAño.Text = DateTime.Now.Year.ToString();
        }

        private void bGenerarEstadistica_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVerificacion.validadorNumeroEntero(tAño.Text))
            {
                DateTime inicio = new DateTime(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1, 1);
                DateTime final = new DateTime(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1, DateTime.DaysInMonth(Convert.ToInt32(tAño.Text), cMes.SelectedIndex + 1));
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
