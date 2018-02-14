using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

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

        private ObservableCollection<GastoNormal> listaGastosNormales;

        private ObservableCollection<GastoBienInversion> listaGastosBienInversion;

        private ObservableCollection<Proveedor> listaProveedores;

        private string tipo;

        public VistaCompletaTabla(string tipo)
        {
            this.InitializeComponent();
            this.tipo = tipo;
            switch (tipo)
            {
                case "camion":
                    {
                        this.listaCamiones = new ObservableCollection<Camion>();
                        this.crearColumnasCamion();
                        IList camiones = CamionesCRUD.cogerTodosCamiones();
                        if (camiones != null)
                        {
                            foreach (Camion item in camiones)
                            {
                                this.listaCamiones.Add(item);
                            }
                        }
                        break;
                    }
                case "empleado":
                    {
                        this.listaEmpleados = new ObservableCollection<Empleado>();
                        this.crearColumnasEmpleado();
                        IList empleados = EmpleadosCRUD.cogerTodosEmpleados();
                        foreach (Empleado item2 in empleados)
                        {
                            Empleado empleado2 = item2;
                            DateTime dateTime = item2.fechaAlta;
                            empleado2.fechaAlta = dateTime.Date;
                            Empleado empleado3 = item2;
                            dateTime = item2.fechaNacimiento;
                            empleado3.fechaNacimiento = dateTime.Date;
                            this.listaEmpleados.Add(item2);
                        }
                        break;
                    }
                case "empresa":
                    {
                        this.listaEmpresas = new ObservableCollection<Empresa>();
                        this.crearColumnasEmpresa();
                        IList empresas = EmpresasCRUD.cogerTodasEmpresas();
                        if (empresas != null)
                        {
                            foreach (Empresa item3 in empresas)
                            {
                                this.listaEmpresas.Add(item3);
                            }
                        }
                        break;
                    }
                case "tarifa":
                    {
                        this.listaTarifas = new ObservableCollection<Tarifa>();
                        this.crearColumnasTarifa();
                        IList tarifas = TarifasCRUD.cogerTodasTarifas();
                        if (tarifas != null)
                        {
                            foreach (Tarifa item4 in tarifas)
                            {
                                this.listaTarifas.Add(item4);
                            }
                        }
                        break;
                    }
                case "alertaFECHA":
                    {
                        this.listaAlertasFecha = new ObservableCollection<AlertaFecha>();
                        this.crearColumnasAlertaFecha();
                        IList alertasFecha = AlertasCRUD.cogerTodasAlertasFecha();
                        if (alertasFecha != null)
                        {
                            foreach (AlertaFecha item5 in alertasFecha)
                            {
                                this.listaAlertasFecha.Add(item5);
                            }
                        }
                        break;
                    }
                case "alertaKILOMETRAJE":
                    {
                        this.listaAlertasKM = new ObservableCollection<AlertaKM>();
                        this.crearColumnasAlertaKM();
                        IList alertasKM = AlertasCRUD.cogerTodasAlertasKM();
                        if (alertasKM != null)
                        {
                            foreach (AlertaKM item6 in alertasKM)
                            {
                                this.listaAlertasKM.Add(item6);
                            }
                        }
                        break;
                    }
                case "gastoBIEN INVERSIÓN":
                    {
                        this.listaGastosBienInversion = new ObservableCollection<GastoBienInversion>();
                        this.crearColumnasGastosBienInversion();
                        IList gastosBienInversion = GastosCRUD.cogerTodosGastosBienInversion();
                        if (gastosBienInversion != null)
                        {
                            foreach (GastoBienInversion item7 in gastosBienInversion)
                            {
                                this.listaGastosBienInversion.Add(item7);
                            }
                        }
                        break;
                    }
                case "gastoNORMAL":
                    {
                        this.listaGastosNormales = new ObservableCollection<GastoNormal>();
                        this.crearColumnasGastosNormal();
                        IList gastosNormales = GastosCRUD.cogerTodosGastosNormal();
                        if (gastosNormales != null)
                        {
                            foreach (GastoNormal item8 in gastosNormales)
                            {
                                this.listaGastosNormales.Add(item8);
                            }
                        }
                        break;
                    }
                case "proveedor":
                    {
                        this.listaProveedores = new ObservableCollection<Proveedor>();
                        this.crearColumnasProveedores();
                        IList proveedores = ProveedoresCRUD.cogerTodosProveedores();
                        if (proveedores != null)
                        {
                            foreach (Proveedor item9 in proveedores)
                            {
                                this.listaProveedores.Add(item9);
                            }
                        }
                        break;
                    }
            }
        }

        private void tabla_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            DataGridClipboardCellContent currentCell = e.ClipboardRowContent[this.tabla.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        private void crearColumnasCamion()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaCamiones
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasEmpleado()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaEmpleados
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasEmpresa()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaEmpresas
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasTarifa()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaTarifas
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasAlertaFecha()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaAlertasFecha
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasAlertaKM()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaAlertasKM
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasGastosNormal()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaGastosNormales
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasGastosBienInversion()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaGastosBienInversion
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasProveedores()
        {
            this.tabla.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = this.listaProveedores
            });
            this.tabla.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void tabla_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("listaComponentesTarifa") || e.Column.Header.ToString().Equals("cifAntiguo") || e.Column.Header.ToString().Equals("personasContacto") || e.Column.Header.ToString().Equals("dniAntiguo") || e.Column.Header.ToString().Equals("nBastidorAntiguo") || e.Column.Header.ToString().Equals("nombreTarifaAntiguo") || e.Column.Header.ToString().Equals("idAntiguo"))
            {
                e.Cancel = true;
            }
            else
            {
                if (e.Column.Header.ToString().Equals("fechaAlta") || e.Column.Header.ToString().Equals("fechaNacimiento") || e.Column.Header.ToString().Equals("fechaLimite") || e.Column.Header.ToString().Equals("fecha"))
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

        private void tabla_AutoGeneratedColumns(object sender, EventArgs e)
        {
            foreach (DataGridColumn column in this.tabla.Columns)
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
                }
            }
        }

    }
}
