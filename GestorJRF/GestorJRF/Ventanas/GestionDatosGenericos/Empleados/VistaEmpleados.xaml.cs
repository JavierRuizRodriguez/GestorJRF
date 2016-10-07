using GestorJRF.Utilidades;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Empleados
{
    /// <summary>
    /// Lógica de interacción para VistaEmpleados.xaml
    /// </summary>
    public partial class VistaEmpleados : Window
    {
        public VistaEmpleados()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }
    }
}
