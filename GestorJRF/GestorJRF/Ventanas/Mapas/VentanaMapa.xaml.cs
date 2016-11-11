using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Mapas;
using GestorJRF.REST_MAPAS;
using GestorJRF.Utilidades;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GestorJRF.Ventanas.Mapas
{
    /// <summary>
    /// Lógica de interacción para VentanaMapa.xaml
    /// </summary>
    public partial class VentanaMapa : Window
    {
        private const string DIRECCION_NAVE = @"Calle La Habana, 28806 Alcalá de Henares";
        private const double LATITUD_NAVE = 40.51272;
        private const double LONGITUD_NAVE = -3.41142;

        private Dispatcher dispacher;
        private char letraItinerario;

        private double kilometrosRutaActual;
        private Tarifa tarifaGeneralCliente;
        private ComponenteTarifa componenteTarifaActual;

        private IList listaEmpresas;
        private List<MapPolyline> rutasActuales;
        private List<Pushpin> banderasActuales;
        private List<Route> rutasPosibles;
        private List<Route> rutasRetornoPosibles;

        public ObservableCollection<Ruta> listaRutas { get; set; }
        public ObservableCollection<Ruta> listaRutasRetorno { get; set; }
        public ObservableCollection<Itinerario> listaItinerarios { get; set; }

        public VentanaMapa()
        {
            listaEmpresas = EmpresasCRUD.cogerTodasEmpresas();
            listaRutas = new ObservableCollection<Ruta>();
            listaRutasRetorno = new ObservableCollection<Ruta>();
            listaItinerarios = new ObservableCollection<Itinerario>();
            listaItinerarios.CollectionChanged += OnCollectionChanged;

            dispacher = Application.Current.Dispatcher;

            letraItinerario = 'A';

            InitializeComponent();
            UtilidadesVentana.SituarVentana(2, this);

            foreach (Empresa empresa in listaEmpresas)
                cCliente.Items.Add(new ComboBoxItem().Content = empresa.nombre.ToUpper());

            banderasActuales = new List<Pushpin>();
            rutasPosibles = new List<Route>();
            rutasRetornoPosibles = new List<Route>();
            rutasActuales = new List<MapPolyline>();
        }

        private char cogerLetraItinerario()
        {
            return letraItinerario++;
        }

        private void bCalcularRuta_Click(object sender, RoutedEventArgs e)
        {
            reiniciarCampos(1);
            dispacher.Invoke(new Action(() => { _calcularRuta(listaItinerarios); }));
        }

        private void _calcularRuta(ObservableCollection<Itinerario> _listaItinerarios)
        {
            if (listaItinerarios != null && listaItinerarios.Count > 1)
            {
                List<Itinerario> itinerarios = new List<Itinerario>(_listaItinerarios);
                GetRoute(false, itinerarios);
                List<Itinerario> listaRetorno = new List<Itinerario>();
                listaRetorno.Add(listaItinerarios[listaItinerarios.Count - 1]);
                listaRetorno.Add(new Itinerario(cogerLetraItinerario(), DIRECCION_NAVE, LATITUD_NAVE, LONGITUD_NAVE, false));
                GetRoute(true, listaRetorno);
            }
            else
            {
                MessageBox.Show("Debe haber más de un itinerario seleccionado para el calcula de la ruta.");
            }
        }

        private void GetKey(Action<string> callback)
        {
            if (callback != null)
            {
                mapa.CredentialsProvider.GetCredentials((c) =>
                {
                    callback(c.ApplicationId);
                });
            }
        }

        private void GetRoute(bool esRetorno, List<Itinerario> itinerario)
        {
            GetKey((c) =>
            {
                List<string> listaOpciones = new List<string>();
                if (checkAutopista.IsChecked == true)
                    listaOpciones.Add("highways");
                if (checkPeaje.IsChecked == true)
                    listaOpciones.Add("tolls");
                BingREST.Route(itinerario, listaOpciones, c, (r) =>
                {
                    if (r != null &&
                        r.ResourceSets != null &&
                        r.ResourceSets.Length > 0 &&
                        r.ResourceSets[0].Resources != null &&
                        r.ResourceSets[0].Resources.Length > 0)
                    {
                        if (!esRetorno)
                        {
                            rutasPosibles.Clear();
                            listaRutas.Clear();
                        }
                        else
                        {
                            rutasRetornoPosibles.Clear();
                            listaRutasRetorno.Clear();
                        }

                        var id = 0;
                        foreach (Route ruta in r.ResourceSets[0].Resources)
                        {
                            if (!esRetorno)
                            {
                                rutasPosibles.Add(ruta);
                                listaRutas.Add(new Ruta(id, ruta.TravelDistance, ruta.TravelDuration));
                            }
                            else
                            {
                                rutasRetornoPosibles.Add(ruta);
                                listaRutasRetorno.Add(new Ruta(id, ruta.TravelDistance, ruta.TravelDuration));
                            }
                            id++;
                        }
                        if (!esRetorno)
                        {
                            tablaRutasPosibles.SelectedIndex = 0;
                            pintarRutas(0);
                            tRutaSeleccionada.Text = "0";
                        }
                        else
                        {
                            tablaRutasRetornoPosibles.SelectedIndex = 0;
                            tRutaRetornoSeleccionada.Text = "0";
                        }


                        gridInformacionPorte.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No Results found.");
                    }
                });
            });

        }

        private void GetLocation(string lugar)
        {
            if (!string.IsNullOrWhiteSpace(lugar))
            {
                GetKey((c) =>
                {
                    BingREST.Location(lugar, c, (r) =>
                    {
                        if (r != null &&
                            r.ResourceSets != null &&
                            r.ResourceSets.Length > 0 &&
                            r.ResourceSets[0].Resources != null &&
                            r.ResourceSets[0].Resources.Length > 0)
                        {
                            REST_MAPAS.Location lugarEncontrado = r.ResourceSets[0].Resources[0] as REST_MAPAS.Location;
                            if (lugarEncontrado.Name.Equals("España") || !lugarEncontrado.Name.Contains("España"))
                            {
                                MessageBox.Show("La dirección solicitada no se encuentra.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                mapa.SetView(new Microsoft.Maps.MapControl.WPF.Location(lugarEncontrado.Point.Coordinates[0], lugarEncontrado.Point.Coordinates[1]), 15, 0);
                                listaItinerarios.Add(new Itinerario(cogerLetraItinerario(), lugarEncontrado.Name, lugarEncontrado.Point.Coordinates[0], lugarEncontrado.Point.Coordinates[1], false));
                                añadirUnaBandera(lugarEncontrado.Point.Coordinates[0], lugarEncontrado.Point.Coordinates[1]);
                                tDireccion.Text = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron resultados.");
                        }
                    });
                });
            }
            else
            {
                MessageBox.Show("Dirección invalida.");
            }
        }

        private void pintarRutas(int idRutaPrincipal)
        {
            limpiarRutasAnteriores();

            var contador = 0;
            foreach (Route r in rutasPosibles)
            {
                if (contador != idRutaPrincipal)
                {
                    _pintarRuta(false, r);
                }
                contador++;

            }
            _pintarRuta(true, rutasPosibles[idRutaPrincipal]);
        }

        private void pintarRutasRetorno(int idRutaPrincipal)
        {
            limpiarRutasAnteriores();

            var contador = 0;
            foreach (Route r in rutasRetornoPosibles)
            {
                if (contador != idRutaPrincipal)
                {
                    _pintarRuta(false, r);
                }
                contador++;

            }
            _pintarRuta(true, rutasRetornoPosibles[idRutaPrincipal]);
        }

        private void _pintarRuta(bool esPrincipal, Route r)
        {
            Route route = r;

            double[][] routePath = route.RoutePath.Line.Coordinates;
            LocationCollection locs = new LocationCollection();

            for (int i = 0; i < routePath.Length; i++)
            {
                if (routePath[i].Length >= 2)
                {
                    locs.Add(new Microsoft.Maps.MapControl.WPF.Location(routePath[i][0], routePath[i][1]));
                }
            }

            MapPolyline routeLine = new MapPolyline()
            {
                Locations = locs,
                Stroke = esPrincipal ? new SolidColorBrush(Colors.Aqua) : new SolidColorBrush(Colors.Red),
                StrokeThickness = 5,
                Opacity = esPrincipal ? 0.7 : 0.3
            };

            rutasActuales.Add(routeLine);
            mapa.Children.Add(routeLine);
            mapa.SetView(locs, new Thickness(200), 0);
            añadirBanderas();
        }

        private void limpiarRutasAnteriores()
        {
            foreach (MapPolyline lineaRuta in rutasActuales)
                mapa.Children.Remove(lineaRuta);
        }

        private void añadirUnaBandera(double latitud, double longitud)
        {
            limpiarBanderas();

            Pushpin bandera = new Pushpin();
            bandera.Location = new Microsoft.Maps.MapControl.WPF.Location(latitud, longitud);
            bandera.Background = new SolidColorBrush(Colors.Orange);
            mapa.Children.Add(bandera);
            banderasActuales.Add(bandera);
        }

        private void añadirBanderas()
        {
            limpiarBanderas();
            foreach (Itinerario itinerario in listaItinerarios)
            {
                Pushpin bandera = new Pushpin();
                bandera.Location = new Microsoft.Maps.MapControl.WPF.Location(itinerario.latitud, itinerario.longitud);
                bandera.Content = itinerario.punto;
                bandera.Background = new SolidColorBrush(Colors.Black);
                mapa.Children.Add(bandera);
                banderasActuales.Add(bandera);
            }
        }

        private void limpiarBanderas()
        {
            foreach (Pushpin bandera in banderasActuales)
                mapa.Children.Remove(bandera);

            banderasActuales.Clear();
        }

        private void tablaRutasPosibles_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null & e.ClickCount > 1)
            {
                tRutaSeleccionada.Text = Convert.ToString(((DataGrid)sender).SelectedIndex);
                limpiarBanderas();
                pintarRutas(((Ruta)((DataGrid)sender).SelectedItem).id);
                calcularTarifacion();
            }
        }

        private void tablaRutasPosibles_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = e.Column.Header.ToString().Equals("id") ? "RUTAS POSIBLES" : UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void esSalidaNave_Checked(object sender, RoutedEventArgs e)
        {
            if (listaItinerarios.Count == 0)
            {
                listaItinerarios.Add(new Itinerario(cogerLetraItinerario(), DIRECCION_NAVE, LATITUD_NAVE, LONGITUD_NAVE, false));
            }
            else
            {
                List<Itinerario> listaItinerarioAux = new List<Itinerario>(listaItinerarios);
                listaItinerarios.Clear();
                listaItinerarios.Add(new Itinerario(cogerLetraItinerario(), DIRECCION_NAVE, LATITUD_NAVE, LONGITUD_NAVE, false));
                foreach (Itinerario i in listaItinerarioAux)
                    listaItinerarios.Add(new Itinerario(cogerLetraItinerario(), i.direccion, i.latitud, i.longitud, false));
            }
        }

        private void esSalidaNave_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listaItinerarios.Count > 0)
                listaItinerarios.RemoveAt(0);
        }

        private void nuevoPuntoItinerario_Click(object sender, RoutedEventArgs e)
        {
            mostraPuntoMapa(tDireccion.Text);
        }

        private void mostraPuntoMapa(string lugar)
        {
            GetLocation(lugar);
        }

        private void tablaItinerario_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("direccionBing") || e.Column.Header.ToString().Equals("latitud")
                || e.Column.Header.ToString().Equals("longitud") || e.Column.Header.ToString().Equals("id")
                || e.Column.Header.ToString().Equals("idResumen") || e.Column.Header.ToString().Equals("dni")
                || e.Column.Header.ToString().Equals("kilometrosVehiculo") || e.Column.Header.ToString().Equals("matricula"))
                e.Cancel = true;

            e.Column.Width = e.Column.Header.ToString().Equals("punto") ? new DataGridLength(50, DataGridLengthUnitType.Pixel) :
                (e.Column.Header.ToString().Equals("esEtapa") ? new DataGridLength(70, DataGridLengthUnitType.Pixel) : new DataGridLength(1, DataGridLengthUnitType.Star));

            e.Column.Header = e.Column.Header.ToString().Equals("esEtapa") ? "es etapa" : e.Column.Header;
            e.Column.Header = UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void tablaItinerario_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.GetType() != typeof(DataGridCheckBoxColumn) || (checkSalidaNave.IsChecked == true && ((DataGrid)sender).SelectedIndex == 0))
                e.Cancel = true;
        }

        private void tablaItinerario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && ((Itinerario)tablaItinerario.SelectedItems[0]).direccion.Equals(DIRECCION_NAVE))
                e.Handled = true;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action.ToString().Equals("Remove"))
                rehacerListaRutas();
            else if (e.Action.ToString().Equals("Reset"))
                letraItinerario = 'A';
        }

        private void rehacerListaRutas()
        {
            letraItinerario = 'A';
            foreach (Itinerario i in listaItinerarios)
                i.punto = cogerLetraItinerario();
        }

        private void cCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cCliente.Items.Count > 0 && gridInformacionPorte.IsEnabled == true && cCliente.SelectedIndex != -1)
            {
                tarifaGeneralCliente = TarifasCRUD.cogerTarifaPorNombreEmpresa(((Empresa)listaEmpresas[cCliente.SelectedIndex]).nombre);

                if (tarifaGeneralCliente == null)
                    tarifaGeneralCliente = TarifasCRUD.cogerTarifaPorNombreEmpresa("GENERAL");

                if (cComponenteTarifa.Items.Count != 0)
                    cComponenteTarifa.Items.Clear();

                foreach (ComponenteTarifa c in tarifaGeneralCliente.listaComponentesTarifa)
                {
                    if (c.tipoCamion.Equals(((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString()))
                        cComponenteTarifa.Items.Add(new ComboBoxItem().Content = c.etiqueta);
                }
                calcularTarifacion();
            }
        }

        private void calcularTarifacion()
        {
            if (checkGrupaje.IsChecked == false && cCliente.SelectedIndex != -1)
            {
                //hacer distinto para empresa rara

                int numeroRepartos = cogerNumeroRepartos();

                tRepartos.Text = Convert.ToString(numeroRepartos);

                kilometrosRutaActual = ((Ruta)tablaRutasPosibles.SelectedItem).kilometros + ((Ruta)tablaRutasRetornoPosibles.SelectedItem).kilometros;

                if (kilometrosRutaActual > 200)
                {
                    componenteTarifaActual = tarifaGeneralCliente.listaComponentesTarifa.Single(c => c.etiqueta == "RADIO +200"
                        && c.tipoCamion == ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString());

                    tPrecioFinal.Text = (componenteTarifaActual.precio * kilometrosRutaActual +
                        numeroRepartos * tarifaGeneralCliente.listaComponentesTarifa.Single(c => c.etiqueta == "REPARTO"
                        && c.tipoCamion == ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString()).precio).ToString("F2");
                    tPrecioTarifa.Text = Convert.ToString(componenteTarifaActual.precio) + " €/KM";
                }
                else if (kilometrosRutaActual < 20)
                {
                    componenteTarifaActual = tarifaGeneralCliente.listaComponentesTarifa.Single(c => c.etiqueta == "URBANA"
                        && c.tipoCamion == ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString());

                    tPrecioFinal.Text = (componenteTarifaActual.precio +
                        numeroRepartos * tarifaGeneralCliente.listaComponentesTarifa.Single(c => c.etiqueta == "REPARTO"
                        && c.tipoCamion == ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString()).precio).ToString("F2");
                    tPrecioTarifa.Text = Convert.ToString(componenteTarifaActual.precio) + " €";
                }
                else
                {
                    kilometrosRutaActual = kilometrosRutaActual / 2;

                    if (kilometrosRutaActual % 10 != 0)
                        kilometrosRutaActual = (kilometrosRutaActual - kilometrosRutaActual % 10) + 10;

                    componenteTarifaActual = tarifaGeneralCliente.listaComponentesTarifa.Single(c => c.etiqueta == "RADIO " + kilometrosRutaActual
                        && c.tipoCamion == ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString());
                    tPrecioFinal.Text = (componenteTarifaActual.precio +
                        numeroRepartos * tarifaGeneralCliente.listaComponentesTarifa.Single(c => c.etiqueta == "REPARTO"
                        && c.tipoCamion == ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString()).precio).ToString("F2");
                    tPrecioTarifa.Text = Convert.ToString(componenteTarifaActual.precio) + " €";
                }
                var id = tarifaGeneralCliente.listaComponentesTarifa.IndexOf(componenteTarifaActual);
                if (componenteTarifaActual.tipoCamion.Equals("CAMIÓN PEQUEÑO"))
                    id -= 22;
                cComponenteTarifa.SelectedIndex = id;
                tPrecioFinal.Text = tPrecioFinal.Text.Replace(",", ".");
                tPrecioTarifa.Text = tPrecioTarifa.Text.Replace(",", ".");
                tNombreTarifa.Text = componenteTarifaActual.nombreTarifa;
            }
        }

        private int cogerNumeroRepartos()
        {
            int repartos = 0;
            foreach (Itinerario i in listaItinerarios)
            {
                if (!i.esEtapa)
                    repartos++;
            }
            if (checkSalidaNave.IsChecked == true)
                return repartos - 2;
            else
                return repartos - 1;
        }

        private void tablaRutasRetornoPosibles_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = e.Column.Header.ToString().Equals("id") ? "RETONO POSIBLE" : UtilidadesVentana.generarEtiquetaFormatoColumna(e.Column.Header.ToString());
        }

        private void tablaRutasRetornoPosibles_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
            {
                tRutaRetornoSeleccionada.Text = Convert.ToString(((DataGrid)sender).SelectedIndex);
                limpiarBanderas();
                pintarRutasRetorno(((Ruta)((DataGrid)sender).SelectedItem).id);
                calcularTarifacion();
            }
        }

        private void cComponenteTarifa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int numeroRepartos = cogerNumeroRepartos();
            if (cComponenteTarifa.Items.Count > 0 && cComponenteTarifa.SelectedIndex != -1)
            {
                int id = cComponenteTarifa.SelectedIndex;
                if (componenteTarifaActual.tipoCamion.Equals("CAMIÓN PEQUEÑO"))
                    id += 22;

                componenteTarifaActual = tarifaGeneralCliente.listaComponentesTarifa[id];

                if (componenteTarifaActual.etiqueta.Equals("RADIO +200"))
                {
                    tPrecioFinal.Text = (componenteTarifaActual.precio * kilometrosRutaActual +
                        numeroRepartos * tarifaGeneralCliente.listaComponentesTarifa.Single(c => c.etiqueta == "REPARTO"
                        && c.tipoCamion == ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString()).precio).ToString("F2");
                    tPrecioTarifa.Text = Convert.ToString(componenteTarifaActual.precio) + " €/KM";
                }
                else
                {
                    tPrecioFinal.Text = (componenteTarifaActual.precio +
                        numeroRepartos * tarifaGeneralCliente.listaComponentesTarifa.Single(c => c.etiqueta == "REPARTO"
                        && c.tipoCamion == ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString()).precio).ToString("F2");
                    tPrecioTarifa.Text = Convert.ToString(componenteTarifaActual.precio) + " €";
                }
            }
        }

        private void cTipoCamion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calcularTarifacion();
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            reiniciarCampos(0);
        }

        private void reiniciarCampos(int modo)
        {
            if (modo == 0)
            {
                UtilidadesVentana.LimpiarCampos(gridItinerario);
                listaItinerarios.Clear();

                checkSalidaNave.IsChecked = false;
                checkSalidaNave.IsChecked = true;
            }

            UtilidadesVentana.LimpiarCampos(gridInfoRutas);
            UtilidadesVentana.LimpiarCampos(gridInfoTransporte);

            listaRutas.Clear();
            listaRutasRetorno.Clear();

            limpiarBanderas();
            limpiarRutasAnteriores();

            gridInformacionPorte.IsEnabled = false;
            cCliente.SelectedIndex = -1;
            cComponenteTarifa.Items.Clear();
        }

        private void bNuevaEmpresa_Click(object sender, RoutedEventArgs e)
        {
            new VentanaGestionEmpresas(true).Show();
        }

        private void bAltaPorte_Click(object sender, RoutedEventArgs e)
        {
            if (UtilidadesVentana.ComprobarCampos(gridInfoTransporte))
            {
                if (UtilidadesVerificacion.validadorFechas(tFechaTransporte.Text) && UtilidadesVerificacion.validadorNumeroDecimal(tPrecioFinal.Text) &&
                    UtilidadesVerificacion.validadorComboBox(cCliente) &&
                    MessageBox.Show("¿Desea añadir el resumen?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Resumen resumenPrevio;
                    if (checkGrupaje.IsChecked == true)
                    {
                        resumenPrevio = new Resumen(((Empresa)listaEmpresas[cCliente.SelectedIndex]).cif, ((Empresa)listaEmpresas[cCliente.SelectedIndex]).nombre,
                        Convert.ToDouble(((Ruta)tablaRutasPosibles.SelectedItem).kilometros, UtilidadesVerificacion.cogerProveedorDecimal()),
                        Convert.ToDouble(((Ruta)tablaRutasRetornoPosibles.SelectedItem).kilometros, UtilidadesVerificacion.cogerProveedorDecimal()),
                        null, null, ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString(), Convert.ToDateTime(tFechaTransporte.Text), listaItinerarios,
                        Convert.ToDouble(tPrecioFinal.Text, UtilidadesVerificacion.cogerProveedorDecimal()));
                    }
                    else
                    {
                        resumenPrevio = new Resumen(((Empresa)listaEmpresas[cCliente.SelectedIndex]).cif, ((Empresa)listaEmpresas[cCliente.SelectedIndex]).nombre,
                        Convert.ToDouble(((Ruta)tablaRutasPosibles.SelectedItem).kilometros, UtilidadesVerificacion.cogerProveedorDecimal()),
                        Convert.ToDouble(((Ruta)tablaRutasRetornoPosibles.SelectedItem).kilometros, UtilidadesVerificacion.cogerProveedorDecimal()),
                        tNombreTarifa.Text, cComponenteTarifa.SelectedItem.ToString(), ((ComboBoxItem)cTipoCamion.SelectedItem).Content.ToString(),
                        Convert.ToDateTime(tFechaTransporte.Text), listaItinerarios, Convert.ToDouble(tPrecioFinal.Text, UtilidadesVerificacion.cogerProveedorDecimal()));
                    }

                    int salida = ResumenesCRUD.añadirResumenPrevio(resumenPrevio);
                    if (salida == 1)
                        reiniciarCampos(0);
                }
            }
            else
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new VentanaMenuPrincipal().Show();
        }

        private void checkGrupaje_Checked(object sender, RoutedEventArgs e)
        {
            cComponenteTarifa.IsEnabled = false;
            tNombreTarifa.Text = "";
            cComponenteTarifa.SelectedIndex = -1;
            tPrecioFinal.Text = "";
            tPrecioTarifa.Text = "";
            tRepartos.Text = "";
        }

        private void checkGrupaje_Unchecked(object sender, RoutedEventArgs e)
        {
            cComponenteTarifa.IsEnabled = true;
            calcularTarifacion();
        }

        private void selectorTipoMapa_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(selectorTipoMapa.Value == 1)
                mapa.Mode = new AerialMode(true);
            else
                mapa.Mode = new RoadMode();
        }
    }
}
