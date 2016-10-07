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

namespace GestorJRF.Ventanas.GestionDatosGenericos
{
    /// <summary>
    /// Lógica de interacción para VistaEmpresas.xaml
    /// </summary>
    public partial class VistaEmpresas : Window
    {
        public VistaEmpresas()
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
