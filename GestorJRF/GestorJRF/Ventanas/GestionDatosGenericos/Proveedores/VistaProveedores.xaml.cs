using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Proveedores;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Proveedores
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionProveedores.xaml
    /// </summary>
    public partial class VistaProveedores : Window
    {
        public Proveedor proveedor
        {
            get;
            set;
        }

        public VistaProveedores()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }
        
        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "proveedor").Show();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (this.proveedor != null)
            {
                if (MessageBox.Show("¿Desea borrar el proveedor?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    int salida = ProveedoresCRUD.borrarProveedor(this.proveedor.cif);
                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                        this.proveedor = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un proveedor.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (this.proveedor != null)
            {
                new VentanaGestionProveedores(this.proveedor).Show();
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                this.proveedor = null;
            }
            else
            {
                MessageBox.Show("Debe seleccionar una alerta para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        internal void MostrarProveedorBuscado()
        {
            this.tCIF.Text = this.proveedor.cif;
            this.tNombre.Text = this.proveedor.nombre;
            this.tDomicilio.Text = this.proveedor.domicilio;
            this.tLocalidad.Text = this.proveedor.localidad;
            this.tProvincia.Text = this.proveedor.provincia;
            this.tCP.Text = this.proveedor.cp.ToString(); ;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }
    }
}
