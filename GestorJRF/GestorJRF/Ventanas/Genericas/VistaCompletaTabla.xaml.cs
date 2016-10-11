using GestorJRF.CRUD.Empresas;
using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using System.Collections;
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
        private ObservableCollection<Alerta> listaAlertas;

        public VistaCompletaTabla(string tipo)
        {
            InitializeComponent();

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
                    IList empleados = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosEmpleados", null);
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
                    crearColumnasTarifa();
                    listaTarifas = new ObservableCollection<Tarifa>();
                    break;
                case "alerta":
                    crearColumnasAlerta();
                    listaAlertas = new ObservableCollection<Alerta>();
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

        private void crearColumnasAlerta()
        {
            tabla.SetBinding(DataGrid.ItemsSourceProperty, new Binding() { Source = listaAlertas });
            tabla.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void tabla_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "cifAntiguo" || e.Column.Header.ToString() == "personasContacto" || e.Column.Header.ToString() == "dniAntiguo" || e.Column.Header.ToString() == "nBastidorAntiguo")
                e.Cancel = true;
            else
            {
                if(e.Column.Header.ToString() == "fechaAlta" || e.Column.Header.ToString() == "fechaNacimiento")
                {
                    DataGridTextColumn columnaNueva = new DataGridTextColumn();
                    columnaNueva.Header = e.Column.Header.ToString();
                    Binding b = new Binding(e.PropertyName);
                    b.StringFormat = "dd/MM/yyyy";
                    columnaNueva.Binding = b;
                    e.Column = columnaNueva;
                }

                e.Column.Header = e.Column.Header.ToString().ToUpper();
            }
        }
    }
}
