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

namespace GestorJRF.Ventanas.GestionDatosGenericos.Tarifas
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionTarifas.xaml
    /// </summary>
    public partial class VentanaGestionTarifas : Window
    {
        public VentanaGestionTarifas()
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
