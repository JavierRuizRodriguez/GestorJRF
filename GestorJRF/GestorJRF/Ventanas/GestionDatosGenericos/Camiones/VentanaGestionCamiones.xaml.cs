using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Camiones
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionCamiones.xaml
    /// </summary>
    public partial class VentanaGestionCamiones : Window
    {
        private bool esAlta;
        private Camion camion;
        public VentanaGestionCamiones()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = true;
        }

        public VentanaGestionCamiones(Camion camion)
        {
            this.InitializeComponent();
            this.esAlta = false;
            this.camion = camion;
            this.tMarca.Text = camion.marca;
            this.tModelo.Text = camion.modelo;
            this.tMatricula.Text = camion.matricula;
            this.tNumBastidor.Text = camion.nBastidor;
            this.tLargoCaja.Text = Convert.ToString(camion.largoCaja);
            this.tLargoVehiculo.Text = Convert.ToString(camion.largoVehiculo);
            this.tKilometraje.Text = Convert.ToString(camion.kilometraje);
            this.tGalibo.Text = Convert.ToString(camion.galibo);
            this.cComustible.SelectedItem = Convert.ToString(camion.tipoCombustible);
            this.bNuevo.Content = "ACTUALIZAR CAMIÓN";
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            if (this.esAlta)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
        }

        private void bNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(this.gridPrincipal))
            {
                if (this.sonCamposValidos())
                {
                    if (this.esAlta)
                    {
                        this.añadirCamion();
                    }
                    else
                    {
                        this.modificarCamion();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private bool sonCamposValidos()
        {
            if (UtilidadesVerificacion.validadorNumeroDecimal(this.tGalibo.Text) || UtilidadesVerificacion.validadorNumeroDecimal(this.tLargoCaja.Text) || UtilidadesVerificacion.validadorNumeroDecimal(this.tLargoVehiculo.Text) || UtilidadesVerificacion.validadorNumeroEntero(this.tKilometraje.Text))
            {
                return true;
            }
            return false;
        }

        private void modificarCamion()
        {
            Camion c = new Camion(this.tMarca.Text, this.tModelo.Text, this.tMatricula.Text, this.tNumBastidor.Text, this.camion.nBastidor, Convert.ToDouble(this.tLargoCaja.Text, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToDouble(this.tLargoVehiculo.Text, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToInt64(this.tKilometraje.Text), Convert.ToDouble(this.tGalibo.Text, UtilidadesVerificacion.cogerProveedorDecimal()), ((ComboBoxItem)this.cComustible.SelectedItem).Content.ToString());
            CamionesCRUD.modificarCamion(c);
        }

        private void añadirCamion()
        {
            Camion c = new Camion(this.tMarca.Text, this.tModelo.Text, this.tMatricula.Text, this.tNumBastidor.Text, Convert.ToDouble(this.tLargoCaja.Text, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToDouble(this.tLargoVehiculo.Text, UtilidadesVerificacion.cogerProveedorDecimal()), Convert.ToInt64(this.tKilometraje.Text), Convert.ToDouble(this.tGalibo.Text, UtilidadesVerificacion.cogerProveedorDecimal()), ((ComboBoxItem)this.cComustible.SelectedItem).Content.ToString());
            int salida = CamionesCRUD.añadirCamion(c);
            if (salida == 1)
            {
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
            }
        }
    }
}
