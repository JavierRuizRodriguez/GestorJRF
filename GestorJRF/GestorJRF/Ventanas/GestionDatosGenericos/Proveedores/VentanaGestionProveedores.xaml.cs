using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System.Windows;
using System;
using GestorJRF.Ventanas.GestionGastos;

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
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            esAlta = true;
            esAltaDesdeGasto = false;
        }

        public VentanaGestionProveedores(VentanaGestionGastos ventanaGastos)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            esAlta = true;
            esAltaDesdeGasto = true;
            this.ventanaGastos = ventanaGastos;
        }

        public VentanaGestionProveedores(Proveedor proveedor)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.proveedor = proveedor;
            esAlta = false;
            esAltaDesdeGasto = false;
            MostrarProveedorBuscado();
        }

        private void MostrarProveedorBuscado()
        {
            tCIF.Text = proveedor.cif;
            tNombre.Text = proveedor.nombre;
            bNuevoProveedor.Content = "MODIFICAR\nPROVEEDOR";
        }

        private void bNuevoAviso_Click(object sender, RoutedEventArgs e)
        {
            if (esAlta)
            {
                if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
                {
                    Proveedor p = new Proveedor(tNombre.Text, tCIF.Text);
                    int salida = ProveedoresCRUD.añadirProveedor(p);

                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);
                        if (esAltaDesdeGasto)
                        {
                            ventanaGastos.actualizarProveedores();
                            Close();
                        }
                    }
                }
            }
            else
            {
                if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
                {
                    Proveedor p = new Proveedor(tNombre.Text, tCIF.Text, proveedor.cif);
                    ProveedoresCRUD.modificarProveedor(p);
                    Close();
                }
            }
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (esAlta && !esAltaDesdeGasto)
                new VentanaMenuGestionDatos().Show();
        }
    }
}
