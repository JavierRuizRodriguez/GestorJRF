using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using GestorJRF.POJOS.Mapas;
using GestorJRF.POJOS.Pagos;
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
using System.Windows.Input;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.Genericas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class VistaCompletaDosTablas : Window
    {
        private ObservableCollection<Resumen> listaResumenes;

        private ObservableCollection<Itinerario> listaItinerarios;

        private ObservableCollection<Empresa> listaEmpresas;

        private ObservableCollection<PersonaContacto> listaPersonalContacto;

        private ObservableCollection<Tarifa> listaTarifas;

        private ObservableCollection<ComponenteTarifa> listaComponentesTarifa;

        private ObservableCollection<Factura> listaFacturas;

        private ObservableCollection<ComponenteFactura> listaComponentesFactura;

        private ObservableCollection<Pago> listaPagos;

        private ObservableCollection<ComponentePago> listaComponentesPago;

        private string tipo;

        public VistaCompletaDosTablas(string tipo)
        {
            this.InitializeComponent();
            this.tipo = tipo;
            switch (tipo)
            {
                case "resumen previo":
                    {
                        this.listaResumenes = new ObservableCollection<Resumen>();
                        this.listaItinerarios = new ObservableCollection<Itinerario>();
                        this.crearColumnasTablaPadre(this.listaResumenes);
                        this.crearColumnasTablaHija(this.listaItinerarios);
                        IList resumenesPrevios = ResumenesCRUD.cogerTodosResumenesPrevios();
                        if (resumenesPrevios != null)
                        {
                            foreach (Resumen item in resumenesPrevios)
                            {
                                this.listaResumenes.Add(item);
                            }
                        }
                        break;
                    }
                case "resumen final":
                    {
                        this.listaResumenes = new ObservableCollection<Resumen>();
                        this.listaItinerarios = new ObservableCollection<Itinerario>();
                        this.crearColumnasTablaPadre(this.listaResumenes);
                        this.crearColumnasTablaHija(this.listaItinerarios);
                        IList resumenesFinales = ResumenesCRUD.cogerTodosResumenesFinales();
                        if (resumenesFinales != null)
                        {
                            foreach (Resumen item2 in resumenesFinales)
                            {
                                this.listaResumenes.Add(item2);
                            }
                        }
                        break;
                    }
                case "tarifa":
                    {
                        this.listaTarifas = new ObservableCollection<Tarifa>();
                        this.listaComponentesTarifa = new ObservableCollection<ComponenteTarifa>();
                        this.crearColumnasTablaPadre(this.listaTarifas);
                        this.crearColumnasTablaHija(this.listaComponentesTarifa);
                        IList tarifas = TarifasCRUD.cogerTodasTarifas();
                        if (tarifas != null)
                        {
                            foreach (Tarifa item3 in tarifas)
                            {
                                this.listaTarifas.Add(item3);
                            }
                        }
                        break;
                    }
                case "empresa":
                    {
                        this.listaEmpresas = new ObservableCollection<Empresa>();
                        this.listaPersonalContacto = new ObservableCollection<PersonaContacto>();
                        this.crearColumnasTablaPadre(this.listaEmpresas);
                        this.crearColumnasTablaHija(this.listaPersonalContacto);
                        IList empresas = EmpresasCRUD.cogerTodasEmpresas();
                        if (empresas != null)
                        {
                            foreach (Empresa item4 in empresas)
                            {
                                this.listaEmpresas.Add(item4);
                            }
                        }
                        break;
                    }
                case "factura":
                    {
                        this.listaFacturas = new ObservableCollection<Factura>();
                        this.listaComponentesFactura = new ObservableCollection<ComponenteFactura>();
                        this.crearColumnasTablaPadre(this.listaFacturas);
                        this.crearColumnasTablaHija(this.listaComponentesFactura);
                        IList facturas = FacturasCRUD.cogerTodasFacturas();
                        if (facturas != null)
                        {
                            foreach (Factura item5 in facturas)
                            {
                                this.listaFacturas.Add(item5);
                            }
                        }
                        break;
                    }
                case "pago":
                case "pagare":
                    {
                        this.listaPagos = new ObservableCollection<Pago>();
                        this.listaComponentesPago = new ObservableCollection<ComponentePago>();
                        this.crearColumnasTablaPadre(this.listaPagos);
                        this.crearColumnasTablaHija(this.listaComponentesPago);
                        IList pagos = (!tipo.Equals("pago")) ? PagosCRUD.cogerTodosPagareSinBanco() : PagosCRUD.cogerTodosPagos();
                        if (pagos != null)
                        {
                            foreach (Pago item6 in pagos)
                            {
                                this.listaPagos.Add(item6);
                            }
                        }
                        break;
                    }
            }
        }

        private void tabla1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            switch (this.tipo)
            {
                case "resumen previo":
                    {
                        this.listaItinerarios.Clear();
                        Resumen resumenSeleccionado2 = (Resumen)((DataGrid)sender).SelectedItem;
                        if (resumenSeleccionado2 != null)
                        {
                            IList itinerariosPrevios = ResumenesCRUD.cogerTodosItinerariosPorIdResumenPrevio(resumenSeleccionado2.id);
                            if (itinerariosPrevios != null)
                            {
                                foreach (Itinerario item in itinerariosPrevios)
                                {
                                    this.listaItinerarios.Add(item);
                                }
                            }
                        }
                        break;
                    }
                case "resumen final":
                    {
                        this.listaItinerarios.Clear();
                        Resumen resumenSeleccionado2 = (Resumen)((DataGrid)sender).SelectedItem;
                        if (resumenSeleccionado2 != null)
                        {
                            IList itinerariosFinal = ResumenesCRUD.cogerTodosItinerariosPorIdResumenFinal(resumenSeleccionado2.id);
                            if (itinerariosFinal != null)
                            {
                                foreach (Itinerario item2 in itinerariosFinal)
                                {
                                    this.listaItinerarios.Add(item2);
                                }
                            }
                        }
                        break;
                    }
                case "tarifa":
                    {
                        this.listaComponentesTarifa.Clear();
                        Tarifa tarifaSeleccionada = (Tarifa)((DataGrid)sender).SelectedItem;
                        if (tarifaSeleccionada != null)
                        {
                            IList componentesTarifa = TarifasCRUD.cogerTodosComponentes(tarifaSeleccionada.nombreTarifa);
                            if (componentesTarifa != null)
                            {
                                foreach (ComponenteTarifa item3 in componentesTarifa)
                                {
                                    this.listaComponentesTarifa.Add(item3);
                                }
                            }
                        }
                        break;
                    }
                case "empresa":
                    {
                        this.listaPersonalContacto.Clear();
                        Empresa empresaSeleccionada = (Empresa)((DataGrid)sender).SelectedItem;
                        if (empresaSeleccionada != null)
                        {
                            IList personasContacto = EmpresasCRUD.cogerTodoPersonalContacto(empresaSeleccionada.cif);
                            if (personasContacto != null)
                            {
                                foreach (PersonaContacto item4 in personasContacto)
                                {
                                    this.listaPersonalContacto.Add(item4);
                                }
                            }
                        }
                        break;
                    }
                case "factura":
                    {
                        this.listaComponentesFactura.Clear();
                        Factura facturaSeleccionada = (Factura)((DataGrid)sender).SelectedItem;
                        if (facturaSeleccionada != null)
                        {
                            IList componentesFactura = FacturasCRUD.cogerTodosComponentes(facturaSeleccionada.numeroFactura);
                            if (componentesFactura != null)
                            {
                                foreach (ComponenteFactura item5 in componentesFactura)
                                {
                                    this.listaComponentesFactura.Add(item5);
                                }
                            }
                        }
                        break;
                    }
                case "pago":
                case "pagare":
                    {
                        this.listaComponentesPago.Clear();
                        Pago pagoSeleccionado = (Pago)((DataGrid)sender).SelectedItem;
                        if (pagoSeleccionado != null)
                        {
                            IList componentesPago = PagosCRUD.cogerTodosComponentesPagoPorId(pagoSeleccionado.id);
                            if (componentesPago != null)
                            {
                                foreach (ComponentePago item6 in componentesPago)
                                {
                                    this.listaComponentesPago.Add(item6);
                                }
                            }
                        }
                        break;
                    }
            }
        }

        private void tabla1_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            DataGridClipboardCellContent currentCell = e.ClipboardRowContent[this.tabla1.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        private void tabla2_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            DataGridClipboardCellContent currentCell = e.ClipboardRowContent[this.tabla2.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        private void crearColumnasTablaPadre(object lista)
        {
            this.tabla1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = lista
            });
            this.tabla1.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void crearColumnasTablaHija(object lista)
        {
            this.tabla2.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
            {
                Source = lista
            });
            this.tabla2.ColumnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        }

        private void tabla1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (this.tipo.Contains("resumen"))
            {
                if (e.Column.Header.ToString().Equals("idAntiguo") || e.Column.Header.ToString().Equals("listaItinerarios") || e.Column.Header.ToString().Equals("cif") || (e.Column.Header.ToString().Equals("referencia") && this.tipo.Equals("resumen previo")) || e.Column.Header.ToString().Equals("listaComisiones"))
                {
                    e.Cancel = true;
                }
                else if (e.Column.Header.ToString().Equals("fechaPorte") || e.Column.Header.ToString().Equals("fechaPagare"))
                {
                    DataGridTextColumn columnaNueva = new DataGridTextColumn();
                    columnaNueva.Header = e.Column.Header.ToString();
                    Binding b = new Binding(e.PropertyName);
                    b.StringFormat = "dd/MM/yyyy";
                    columnaNueva.Binding = b;
                    e.Column = columnaNueva;
                }
            }
            else if (this.tipo.Equals("empresa"))
            {
                if (e.Column.Header.ToString().Equals("cifAntiguo"))
                {
                    e.Cancel = true;
                }
            }
            else if (this.tipo.Equals("tarifa"))
            {
                if (e.Column.Header.ToString().Equals("nombreTarifaAntiguo"))
                {
                    e.Cancel = true;
                }
            }
            else if (this.tipo.Equals("pagare"))
            {
                if (e.Column.Header.ToString().Equals("facturas"))
                {
                    e.Cancel = true;
                }
            }
            else if (e.Column.Header.ToString().Equals("resumenes"))
            {
                e.Cancel = true;
            }
            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void tabla2_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (this.tipo.Contains("resumen"))
            {
                if (e.Column.Header.ToString().Equals("id") || e.Column.Header.ToString().Equals("latitud") || (e.Column.Header.ToString().Equals("dni") && this.tipo.Equals("resumen previo")) || e.Column.Header.ToString().Equals("longitud") || e.Column.Header.ToString().Equals("idResumen"))
                {
                    e.Cancel = true;
                }
                else if (e.Column.Header.ToString().Equals("fechaPorte"))
                {
                    DataGridTextColumn columnaNueva = new DataGridTextColumn();
                    columnaNueva.Header = e.Column.Header.ToString();
                    Binding b = new Binding(e.PropertyName);
                    b.StringFormat = "dd/MM/yyyy";
                    columnaNueva.Binding = b;
                    e.Column = columnaNueva;
                }
            }
            else if (this.tipo.Equals("empresa"))
            {
                if (e.Column.Header.ToString().Equals("cif"))
                {
                    e.Cancel = true;
                }
            }
            else if (this.tipo.Equals("pagare") && e.Column.Header.ToString().Equals("idPago"))
            {
                e.Cancel = true;
            }
            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }
    }
}
