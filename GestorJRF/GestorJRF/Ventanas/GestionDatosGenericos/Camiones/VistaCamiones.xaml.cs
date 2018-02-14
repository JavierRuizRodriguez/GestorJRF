using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
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
    /// Lógica de interacción para VistaCamion.xaml
    /// </summary>
    public partial class VistaCamiones : Window
    {
        public Camion camion
        {
            get;
            set;
        }

        public VistaCamiones()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void CerrandoVentana(object sender, CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "camion").Show();
        }

        internal void MostrarCamionBuscado()
        {
            this.tMarca.Text = this.camion.marca;
            this.tModelo.Text = this.camion.modelo;
            this.tMatricula.Text = this.camion.matricula;
            this.tNumBastidor.Text = this.camion.nBastidor;
            this.tLargoCaja.Text = Convert.ToString(this.camion.largoCaja);
            this.tLargoVehiculo.Text = Convert.ToString(this.camion.largoVehiculo);
            this.tKilometraje.Text = Convert.ToString(this.camion.kilometraje);
            this.tGalibo.Text = Convert.ToString(this.camion.galibo);
            this.tTipoCombustible.Text = this.camion.tipoCombustible;
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (this.camion != null)
            {
                new VentanaGestionCamiones(this.camion).Show();
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                this.camion = null;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un camión para modificarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (this.camion != null)
            {
                if (MessageBox.Show("¿Desea borrar el camión?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    int salida = CamionesCRUD.borrarCamion(this.camion.nBastidor);
                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                        this.camion = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un camión para borrarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
