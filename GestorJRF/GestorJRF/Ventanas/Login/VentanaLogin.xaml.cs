using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.Login
{
    public partial class VentanaLogin : Window
    {
        private static VentanaLogin instancia;

        public VentanaLogin()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.tUsuario.Focus();
            this.hipervinculo.Inlines.Add("Modificar usuario");
            this.hipervinculo1.Inlines.Add("Crear nuevo usuario");
        }

        internal static VentanaLogin getInstancia()
        {
            if (VentanaLogin.instancia == null)
            {
                VentanaLogin.instancia = new VentanaLogin();
            }
            return VentanaLogin.instancia;
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
            this.logIn();
        }

        private void tContraseña_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.logIn();
            }
        }

        private void logIn()
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridPrincipal))
            {
                UsuarioSistema usuario = new UsuarioSistema(this.tUsuario.Text, this.tContraseña.Password, "");
                object usuarioBuscado = UsuarioSistemaCRUD.cogerUsuarioSistema(usuario);
                if (usuarioBuscado != null)
                {
                    new VentanaMenuPrincipal().Show();
                    new AvisoAlerta().Show();
                    base.Close();
                }
                else
                {
                    MessageBox.Show("El usuario o la contraseña son incorrectos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    this.tUsuario.Text = "";
                    this.tContraseña.Password = "";
                    this.tUsuario.Focus();
                }
            }
            else
            {
                MessageBox.Show("Debe introducir el usuario y la contraseña.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}