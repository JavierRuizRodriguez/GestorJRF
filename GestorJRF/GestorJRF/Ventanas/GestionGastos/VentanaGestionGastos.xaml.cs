using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas.GestionDatosGenericos.Proveedores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.GestionGastos
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionGastos.xaml
    /// </summary>
    public partial class VentanaGestionGastos : Window
    {
        private Gasto gasto;
        private bool esAlta;
        private List<Empleado> listaEmpleados;
        private List<Camion> listaVehiculos;
        private List<Proveedor> listaProveedores;

        public VentanaGestionGastos()
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            esAlta = true;
            inicializarComboboxes();
            inicializarIndicesComboboxes();
        }

        public VentanaGestionGastos(Gasto gasto)
        {
            InitializeComponent();
            this.gasto = gasto;
            UtilidadesVentana.SituarVentana(0, this);
            esAlta = false;
            inicializarComboboxes();
            inicializarIndicesComboboxes();
            tFecha.Text = gasto.fecha.Date.ToString("dd/MM/yyyy");
            tDescripcion.Text = gasto.descripcion;
            tIva.Text = Convert.ToString(gasto.iva);
            tIrpf.Text = Convert.ToString(gasto.irpf);
            tReferencia.Text = gasto.referencia;
            tFecha.Text = gasto.fecha.Date.ToString("dd/MM/yyyy");
            if (gasto.concepto.Equals("MANUTENCIÓN"))
            {
                tImporte.Text = Convert.ToString(gasto.cuotaDeducible);
                lImporte.Content = "Cuota deducible";
            }
            else
                tImporte.Text = Convert.ToString(gasto.importeBase);
            tImporte.Text = tImporte.Text.Replace(",", ".");
            bNuevoGasto.Content = "MODIFICAR GASTO";
        }

        private void inicializarIndicesComboboxes()
        {
            if (esAlta)
            {
                cVehiculo.SelectedIndex = 0;
                cEmpleado.SelectedIndex = 0;
                cConcepto.SelectedIndex = 0;
                cProveedor.SelectedIndex = 0;
            }
            else
            {
                if (gasto.dni == null || gasto.bastidor == null)
                    checkGastoGenerico.IsChecked = true;
                else
                {
                    cVehiculo.SelectedIndex = buscarIndiceVehiculo();
                    cEmpleado.SelectedIndex = buscarIndiceEmpleado();
                }
                switch (gasto.concepto)
                {
                    case "COMBUSTIBLE":
                        cConcepto.SelectedIndex = 0;
                        break;
                    case "MANUTENCIÓN":
                        cConcepto.SelectedIndex = 1;
                        break;
                    case "MANTENIMIENTO":
                        cConcepto.SelectedIndex = 2;
                        break;
                    case "DOCUMENTACIÓN":
                        cConcepto.SelectedIndex = 3;
                        break;
                    case "TRANSPORTE":
                        cConcepto.SelectedIndex = 4;
                        break;
                    case "OTROS":
                        cConcepto.SelectedIndex = 5;
                        break;
                    default: break;
                }
                cProveedor.SelectedIndex = buscarIndiceProveedor();
            }
        }

        private int buscarIndiceProveedor()
        {
            int contador = 0;
            foreach (Proveedor p in listaProveedores)
            {
                if (p.nombre.Equals(gasto.proveedor))
                    return contador;
                else
                    contador++;
            }
            return contador;
        }

        private int buscarIndiceVehiculo()
        {
            int contador = 0;
            foreach (Camion c in listaVehiculos)
            {
                if (c.nBastidor.Equals(gasto.bastidor))
                    return contador;
                else
                    contador++;
            }
            return contador;
        }

        private int buscarIndiceEmpleado()
        {
            int contador = 0;
            foreach (Empleado e in listaEmpleados)
            {
                if (e.dni.Equals(gasto.dni))
                    return contador;
                else
                    contador++;
            }
            return contador;
        }

        private void inicializarComboboxes()
        {
            listaEmpleados = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
            if (listaEmpleados != null)
            {
                listaEmpleados = new List<Empleado>(listaEmpleados.OrderBy(e => e.nombre));
                foreach (Empleado e in listaEmpleados)
                    cEmpleado.Items.Add(new ComboBoxItem().Content = e.getNombreApellidos());
            }

            listaVehiculos = CamionesCRUD.cogerTodosCamiones().Cast<Camion>().ToList();
            if (listaVehiculos != null)
            {
                listaVehiculos = new List<Camion>(listaVehiculos.OrderBy(c => c.matricula));
                foreach (Camion c in listaVehiculos)
                    cVehiculo.Items.Add(new ComboBoxItem().Content = c.matricula.ToUpper());
            }

            listaProveedores = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToList();
            if (listaProveedores != null)
            {
                listaProveedores = new List<Proveedor>(listaProveedores.OrderBy(p => p.nombre));
                foreach (Proveedor p in listaProveedores)
                    cProveedor.Items.Add(new ComboBoxItem().Content = p.nombre);
            }
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            if (esAlta)
                new VentanaMenuPrincipal().Show();
        }

        private void checkGastoGenerico_Checked(object sender, RoutedEventArgs e)
        {
            cEmpleado.IsEnabled = false;
            cVehiculo.IsEnabled = false;
        }

        private void checkGastoGenerico_Unchecked(object sender, RoutedEventArgs e)
        {
            cEmpleado.IsEnabled = true;
            cVehiculo.IsEnabled = true;
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(gridPrincipal);
        }

        private void bNuevoGasto_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridPrincipal))
            {
                if (UtilidadesVerificacion.validadorFechas(tFecha.Text) && UtilidadesVerificacion.validadorNumeroDecimal(tImporte.Text)
                    && UtilidadesVerificacion.validadorNumeroEntero(tIva.Text) && UtilidadesVerificacion.validadorNumeroEntero(tIrpf.Text))
                {
                    if (esAlta)
                        añadirGasto();
                    else
                        modificarGasto();
                }
            }
            else
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void modificarGasto()
        {
            Gasto g; double cuotaDeducible;
            double importeBase;
            int iva = Convert.ToInt32(tIva.Text);
            int irpf = Convert.ToInt32(tIrpf.Text);

            if (cConcepto.SelectedIndex == 1)
            {
                cuotaDeducible = Convert.ToDouble(tImporte.Text, UtilidadesVerificacion.cogerProveedorDecimal());
                importeBase = (cuotaDeducible * 100) / (100 + iva);
            }
            else
            {
                importeBase = Convert.ToDouble(tImporte.Text, UtilidadesVerificacion.cogerProveedorDecimal());
                cuotaDeducible = importeBase + ((importeBase * iva / 100) - (importeBase * irpf / 100));
            }

            if (checkGastoGenerico.IsChecked == true)
            {
                g = new Gasto(gasto.id, null, null, ((ComboBoxItem)cConcepto.SelectedItem).Content.ToString(),
                    Convert.ToDateTime(tFecha.Text), tDescripcion.Text, importeBase, cuotaDeducible, iva, irpf,
                    tReferencia.Text, listaProveedores[cProveedor.SelectedIndex].nombre, listaProveedores[cProveedor.SelectedIndex].cif);
            }
            else
            {
                g = new Gasto(gasto.id, ((Empleado)listaEmpleados[cEmpleado.SelectedIndex]).dni, ((Camion)listaVehiculos[cVehiculo.SelectedIndex]).nBastidor,
                    ((ComboBoxItem)cConcepto.SelectedItem).Content.ToString(), Convert.ToDateTime(tFecha.Text), tDescripcion.Text, importeBase, cuotaDeducible,
                    iva, irpf, tReferencia.Text, listaProveedores[cProveedor.SelectedIndex].nombre, listaProveedores[cProveedor.SelectedIndex].cif);
            }

            if (GastosCRUD.modificarGasto(g) == 1)
                Close();
        }

        private void añadirGasto()
        {
            Gasto g;
            double cuotaDeducible;
            double importeBase;
            int iva = Convert.ToInt32(tIva.Text);
            int irpf = Convert.ToInt32(tIrpf.Text);

            if (cConcepto.SelectedIndex == 1)
            {
                cuotaDeducible = Convert.ToDouble(tImporte.Text, UtilidadesVerificacion.cogerProveedorDecimal());
                importeBase = (cuotaDeducible * 100) / (100 + iva);
            }
            else
            {
                importeBase = Convert.ToDouble(tImporte.Text, UtilidadesVerificacion.cogerProveedorDecimal());
                cuotaDeducible = importeBase + ((importeBase * iva / 100) - (importeBase * irpf / 100));
            }

            if (checkGastoGenerico.IsChecked == true)
            {
                g = new Gasto(null, null, ((ComboBoxItem)cConcepto.SelectedItem).Content.ToString(),
                    Convert.ToDateTime(tFecha.Text), tDescripcion.Text, importeBase, cuotaDeducible,
                    iva, irpf, tReferencia.Text, listaProveedores[cProveedor.SelectedIndex].nombre, listaProveedores[cProveedor.SelectedIndex].cif);
            }
            else
            {
                g = new Gasto(((Empleado)listaEmpleados[cEmpleado.SelectedIndex]).dni, ((Camion)listaVehiculos[cVehiculo.SelectedIndex]).nBastidor,
                    ((ComboBoxItem)cConcepto.SelectedItem).Content.ToString(), Convert.ToDateTime(tFecha.Text), tDescripcion.Text,
                    importeBase, cuotaDeducible, iva, irpf, tReferencia.Text, listaProveedores[cProveedor.SelectedIndex].nombre, listaProveedores[cProveedor.SelectedIndex].cif);
            }

            if (GastosCRUD.añadirGasto(g) == 1)
            {
                UtilidadesVentana.LimpiarCampos(gridPrincipal);
                inicializarIndicesComboboxes();
            }
        }

        private void cConcepto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cConcepto.SelectedIndex == 1)
                lImporte.Content = "Cuota deducible";
            else
                lImporte.Content = "Importe base";
        }

        private void bNuevoProveedor_Click(object sender, RoutedEventArgs e)
        {
            new VentanaGestionProveedores(this).Show();
        }

        public void actualizarProveedores()
        {
            cProveedor.Items.Clear();
            listaProveedores = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToList();
            if (listaProveedores != null)
            {
                listaProveedores = new List<Proveedor>(listaProveedores.OrderBy(p => p.nombre));
                foreach (Proveedor p in listaProveedores)
                    cProveedor.Items.Add(new ComboBoxItem().Content = p.nombre);
            }
            if (cProveedor.Items.Count > 0)
                cProveedor.SelectedIndex = 0;
        }
    }
}
