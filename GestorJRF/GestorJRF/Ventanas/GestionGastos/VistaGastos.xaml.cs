using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos;
using System;
using System.Windows;

namespace GestorJRF.Ventanas.GestionGastos
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionGastos.xaml
    /// </summary>
    public partial class VistaGastos : Window
    {
        public Gasto gasto { get; set; }

        public VistaGastos()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            new VentanaMenuPrincipal().Show();
        }

        private void bModificarGasto_Click(object sender, RoutedEventArgs e)
        {
            new VentanaGestionGastos(gasto).Show();
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
            gasto = null;
        }

        private void bBorrarGasto_Click(object sender, RoutedEventArgs e)
        {
            if (gasto != null)
            {
                if (MessageBox.Show("¿Desea borrar el gasto?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (GastosCRUD.borrarGasto(gasto.id) == 1)
                    {
                        UtilidadesVentana.LimpiarCampos(gridPrincipal);
                        gasto = null;
                    }
                }
            }
            else
                MessageBox.Show("Debe seleccionar un gasto para borrarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bBuscarGasto_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "gasto").Show();
        }

        internal void MostrarGastoBuscado()
        {
            if (gasto != null)
            {
                if (gasto.bastidor == null || gasto.dni == null)
                {
                    checkGastoGenerico.IsChecked = true;
                }
                else
                {
                    tVehiculo.Text = buscarMatriculaVehiculo();
                    tEmpleado.Text = buscarNombreApellidosEmpleado();
                }
                tConcepto.Text = gasto.concepto;
                tFecha.Text = gasto.fecha.Date.ToString("dd/MM/yyyy");
                tDescripcion.Text = gasto.descripcion;
                tIva.Text = Convert.ToString(gasto.iva);
                tIrpf.Text = Convert.ToString(gasto.irpf);
                tReferencia.Text = gasto.referencia;
                tFecha.Text = gasto.fecha.Date.ToString("dd/MM/yyyy");
                tProveedor.Text = gasto.proveedor;
                if (gasto.concepto.Equals("MANUTENCIÓN"))
                {
                    tImporte.Text = Convert.ToString(gasto.cuotaDeducible);
                    lImporte.Content = "Cuota deducible";
                }
                else
                    tImporte.Text = Convert.ToString(gasto.importeBase);
            }
        }

        private string buscarMatriculaVehiculo()
        {
            Camion camion = CamionesCRUD.cogerCamion("BASTIDOR", gasto.bastidor);
            if (camion != null)
                return camion.matricula;
            else
                return "NO ENCONTRADO";
        }

        private string buscarNombreApellidosEmpleado()
        {
            Empleado empleado = EmpleadosCRUD.cogerEmpleado("DNI", gasto.dni);
            if (empleado != null)
                return empleado.getNombreApellidos();
            else
                return "NO ENCONTRADO";
        }
    }
}
