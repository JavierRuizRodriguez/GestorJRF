using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Gastos;
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
    public partial class VentanaGestionProveedores : Window
    {
        private VentanaGestionGastos ventanaGastos;

        private Proveedor proveedor;

        private bool esAlta;

        private bool esAltaDesdeGasto;

        public VentanaGestionProveedores()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = true;
            this.esAltaDesdeGasto = false;
        }

        public VentanaGestionProveedores(VentanaGestionGastos ventanaGastos)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = true;
            this.esAltaDesdeGasto = true;
            this.ventanaGastos = ventanaGastos;
        }

        public VentanaGestionProveedores(Proveedor proveedor)
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.proveedor = proveedor;
            this.esAlta = false;
            this.esAltaDesdeGasto = false;
            this.MostrarProveedorBuscado();
        }

        private void MostrarProveedorBuscado()
        {
            this.tCIF.Text = this.proveedor.cif;
            this.tNombre.Text = this.proveedor.nombre;
            this.tDomicilio.Text = this.proveedor.domicilio;
            this.tLocalidad.Text = this.proveedor.localidad;
            this.tProvincia.Text = this.proveedor.provincia;
            this.tCP.Text = this.proveedor.cp.ToString();
            this.bNuevoProveedor.Content = "MODIFICAR\nPROVEEDOR";
        }

        private void bNuevoAviso_Click(object sender, RoutedEventArgs e)
        {
            if (this.esAlta)
            {
                if (UtilidadesVentana.ComprobarCampos(this.gridPrincipal) && UtilidadesVerificacion.validadorNumeroEntero(this.tCP.Text))
                {
                    Proveedor p2 = new Proveedor(this.tNombre.Text, this.tCIF.Text, this.tDomicilio.Text, this.tLocalidad.Text, this.tProvincia.Text, Convert.ToInt32(this.tCP.Text));
                    int salida = ProveedoresCRUD.añadirProveedor(p2);
                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                        if (this.esAltaDesdeGasto)
                        {
                            this.ventanaGastos.actualizarProveedores();
                            base.Close();
                        }
                    }
                }
            }
            else if (UtilidadesVentana.ComprobarCampos(this.gridPrincipal) && UtilidadesVerificacion.validadorNumeroEntero(this.tCP.Text))
            {
                Proveedor p = new Proveedor(this.tNombre.Text, this.tCIF.Text, this.proveedor.cif, this.tDomicilio.Text, this.tLocalidad.Text, this.tProvincia.Text, Convert.ToInt32(this.tCP.Text));
                ProveedoresCRUD.modificarProveedor(p);
                base.Close();
            }
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.esAlta && !this.esAltaDesdeGasto)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }
    }
}
