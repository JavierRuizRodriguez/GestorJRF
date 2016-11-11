using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
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
            UtilidadesVentana.SituarVentana(0, this);

            hipervinculo.Inlines.Add("Modificar la contraseña");
            hipervinculo1.Inlines.Add("Crear nuevo usuario");
        }

        internal static VentanaLogin getInstancia()
        {
            if (instancia == null)
                instancia = new VentanaLogin();

            return instancia;
        }

        private void hipervinculo_Click(object sender, RoutedEventArgs e)
        {
            new VentanaOpcionesModificacionUsuario().Show();
        }

        private void hipervinculo1_Click(object sender, RoutedEventArgs e)
        {
            new VentanaNuevoUsuarioSistema().Show();
        }

        private void bEntrar_Click(object sender, RoutedEventArgs e)
        {
            UsuarioSistema usuario = new UsuarioSistema(tUsuario.Text, tContraseña.Password, "");
            Object usuarioBuscado = UsuarioSistemaCRUD.cogerUsuarioSistema(usuario);
            if (usuarioBuscado != null)
            {
                new VentanaMenuPrincipal().Show();
                new AvisoAlerta().Show();
                Close();
            }
            else
            {
                MessageBox.Show("El usuario o la contraseña son incorrectos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                tUsuario.Text = "";
                tContraseña.Password = "";
            }
        }
    }
}
