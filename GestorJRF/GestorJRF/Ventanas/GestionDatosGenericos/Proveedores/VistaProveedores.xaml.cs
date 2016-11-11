using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System.Windows;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Proveedores
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionProveedores.xaml
    /// </summary>
    public partial class VistaProveedores : Window
    {
        public Proveedor proveedor { get; set; }
        public VistaProveedores()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0,this);
        }

        private void bNuevoAviso_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
            {
                Proveedor proveedor = new Proveedor(tNombre.Text, tCIF.Text);
                int salida = ProveedoresCRUD.añadirProveedor(proveedor);

                if (salida == 1)
                    UtilidadesVentana.LimpiarCampos(gridPrincipal);
            }
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "proveedor").Show();
        }

        private void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (proveedor != null)
            {
                if (MessageBox.Show("¿Desea borrar el proveedor?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int salida = ProveedoresCRUD.borrarProveedor(proveedor.cif);

                    if (salida == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);
                        proveedor = null;
                    }
                }
            }
            else
                MessageBox.Show("Debe seleccionar un proveedor.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bModificar_Click(object sender, RoutedEventArgs e)
        {
            if (proveedor != null)
            {
                new VentanaGestionProveedores(proveedor).Show();
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
                proveedor = null;
            }
            else
                MessageBox.Show("Debe seleccionar una alerta para borrarla.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        internal void MostrarProveedorBuscado()
        {
            tCIF.Text = proveedor.cif;
            tNombre.Text = proveedor.nombre;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }
    }
}
