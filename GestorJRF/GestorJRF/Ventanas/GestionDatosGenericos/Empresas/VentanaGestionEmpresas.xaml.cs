using GestorJRF.POJOS;
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

namespace GestorJRF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionEmpleados.xaml
    /// </summary>
    public partial class VentanaGestionEmpresas : Window
    {
        private bool esAlta;

        public VentanaGestionEmpresas(Empresa empresa)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
            this.esAlta = false;
            pasarModificacionUI(empresa);
        }

        public VentanaGestionEmpresas()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
            this.esAlta = true;
        }

        private void pasarModificacionUI(Empresa empresa)
        {
            bNuevaEmpresa.Content = "MODIFICAR EMPRESA";
            //modificar campos con datos de la empresa;
        }

        private void CerrandoVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }
    }
}
