using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestorJRF
{
    public partial class VentanaLogin : Window
    {
        private static VentanaLogin instancia;

        public VentanaLogin()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(this);
        }

        internal static VentanaLogin getInstancia()
        {
            if (instancia == null)
                instancia = new VentanaLogin();

            return instancia;
        }

        private void bEntrar_Click(object sender, RoutedEventArgs e)
        {
            //hacer comprobación usuario contraseña.
            VentanaMenuPrincipal vMenuPrincipal = new VentanaMenuPrincipal();
            vMenuPrincipal.Show();
            this.Close();
        }
    }
}
