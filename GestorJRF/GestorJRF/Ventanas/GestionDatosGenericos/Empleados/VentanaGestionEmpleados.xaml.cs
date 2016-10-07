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

namespace GestorJRF.Ventanas.GestionDatosGenericos.Empleados
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionEmpleados.xaml
    /// </summary>
    public partial class VentanaGestionEmpleados : Window
    {
        public VentanaGestionEmpleados()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private void bNuevoEmpleado_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
