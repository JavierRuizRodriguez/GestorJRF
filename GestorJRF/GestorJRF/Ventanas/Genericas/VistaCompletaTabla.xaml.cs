using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GestorJRF.Ventanas.Genericas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class VistaCompletaTabla : Window
    {
        private ObservableCollection<Camion> listaCamiones;
        private ObservableCollection<Empleado> listaEmpleados;
        private ObservableCollection<Empresa> listaEmpresas;
        private ObservableCollection<Tarifa> listaTarifas;
        private ObservableCollection<AlertaFecha> listaAlertasFecha;
        private ObservableCollection<AlertaKM> listaAlertasKM;
        private ObservableCollection<Gasto> listaGastos;
        private ObservableCollection<Proveedor> listaProveedores;

        private string tipo;

        public VistaCompletaTabla(string tipo)
        {
            InitializeComponent();
            this.tipo = tipo;

            switch (tipo)
            {
                case "camion":
                    listaCamiones = new ObservableCollection<Camion>();
                    crearColumnasCamion();
                    IList camiones = CamionesCRUD.cogerTodosCamiones();
                    if (camiones != null)
                    {
                        foreach (Camion camion in camiones)
                            listaCamiones.Add(camion);
                    }
                    break;
                case "empleado":
                    listaEmpleados = new ObservableCollection<Empleado>();
                    crearColumnasEmpleado();
                    IList empleados = EmpleadosCRUD.cogerTodosEmpleados();
                    foreach (Empleado empleado in empleados)
                    {
                        empleado.fechaAlta = empleado.fechaAlta.Date;
                        empleado.fechaNacimiento = empleado.fechaNacimiento.Date;
                        listaEmpleados.Add(empleado);
                    }
                    break;
                case "empresa":
                    listaEmpresas = new ObservableCollection<Empresa>();
                    crearColumnasEmpresa();
                    IList empresas = EmpresasCRUD.cogerTodasEmpresas();
                    if (empresas != null)
                    {
                        foreach (Empresa empresa in empresas)
                            listaEmpresas.Add(empresa);
                    }
                    break;
                case "tarifa":
                    listaTarifas = new ObservableCollection<Tarifa>();
                    crearColumnasTarifa();
                    IList tarifas = TarifasCRUD.cogerTodasTarifas();
                    if (tarifas != null)
                    {
                        foreach (Tarifa tarifa in tarifas)
                            listaTarifas.Add(tarifa);
                    }
                    break;
                case "alertaFECHA":
                    listaAlertasFecha = new ObservableCollection<AlertaFecha>();
                    crearColumnasAlertaFecha();
                    IList alertasFecha = AlertasCRUD.cogerTodasAlertasFecha();
                    if (alertasFecha != null)
                    {
                        foreach (AlertaFecha alerta in alertasFecha)
                            listaAlertasFecha.Add(alerta);
                    }
                    break;
                case "alertaKILOMETRAJE":
                    listaAlertasKM = new ObservableCollection<AlertaKM>();
                    crearColumnasAlertaKM();
                    IList alertasKM = AlertasCRUD.cogerTodasAlertasKM();
                    if (alertasKM != null)
                    {
                        foreach (AlertaKM alerta in alertasKM)
                            listaAlertasKM.Add(alerta);
                    }
                    break;
                case "gasto":
                    listaGastos = new ObservableCollection<Gasto>();
                    crearColumnasGastos();
                    IList gastos = GastosCRUD.cogerTodosGastos();
                    if (gastos != null)
                    {
                        foreach (Gasto gasto in gastos)
                            listaGastos.Add(gasto);
                    }
                    break;

                case "proveedor":
                    listaProveedores = new ObservableCollection<Proveedor>();
                    crearColumnasProveedores();
                    IList proveedores = ProveedoresCRUD.cogerTodosProveedores();
                    if (proveedores != null)
                    {
                        foreach (Proveedor proveedor in proveedores)
                            listaProveedores.Add(proveedor);
                    }
                    break;
                default:
                    break;
            }
        }

        private void tabla_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[tabla.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        private void crearColumnasCamion()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaCamiones });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasEmpleado()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaEmpleados });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasEmpresa()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaEmpresas });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasTarifa()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaTarifas });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasAlertaFecha()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaAlertasFecha });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasAlertaKM()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaAlertasKM });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasGastos()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaGastos });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void crearColumnasProveedores()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaProveedores });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void tabla_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "listaComponentesTarifa" || e.Column.Header.ToString() == "cifAntiguo" ||
                e.Column.Header.ToString() == "personasContacto" || e.Column.Header.ToString() == "dniAntiguo" ||
                e.Column.Header.ToString() == "nBastidorAntiguo" || e.Column.Header.ToString() == "nombreTarifaAntiguo" ||
                e.Column.Header.ToString() == "idAntiguo")
                e.Cancel = true;
            else
            {
                if (e.Column.Header.ToString() == "fechaAlta" || e.Column.Header.ToString() == "fechaNacimiento" ||
                    e.Column.Header.ToString() == "fechaLimite" || e.Column.Header.ToString() == "fecha")
                {
                    DataGridTextColumn columnaNueva = new DataGridTextColumn();
                    columnaNueva.Header = e.Column.Header.ToString();
                    Binding b = new Binding(e.PropertyName);
                    b.StringFormat = "dd/MM/yyyy";
                    columnaNueva.Binding = b;
                    e.Column = columnaNueva;
                }

                e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
            }
        }

        private void tabla_AutoGeneratedColumns(object sender, System.EventArgs e)
        {
            foreach (DataGridColumn column in tabla.Columns)
            {
                switch (column.Header.ToString())
                {
                    case "DIAS ANTELACION":
                        column.DisplayIndex = 5;
                        break;
                    case "FECHA LIMITE":
                        column.DisplayIndex = 5;
                        break;
                    case "KM LIMITE":
                        column.DisplayIndex = 5;
                        break;
                    case "KM ANTELACION":
                        column.DisplayIndex = 5;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
