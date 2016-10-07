using GestorJRF.Utilidades;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Tarifas
{
    /// <summary>
    /// Lógica de interacción para VistaTarifas.xaml
    /// </summary>
    public partial class VistaTarifas : Window
    {
        public VistaTarifas()
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
