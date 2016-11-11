using GestorJRF.CRUD;
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
    /// Lógica de interacción para VentanaNuevoUsuarioSistema.xaml
    /// </summary>
    public partial class VentanaNuevoUsuarioSistema : Window
    {
        public VentanaNuevoUsuarioSistema()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void bNuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal) && !tContraseña.Password.Equals("") && UtilidadesVerificacion.validadorMail(tEmail.Text))
            {
                var salida = UsuarioSistemaCRUD.añadirUsuarioSistema(new UsuarioSistema(tUsuario.Text, tContraseña.Password, tEmail.Text));
                if (salida == 1)
                    Close();
            }
        }
    }
}
