using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas.Genericas;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using GestorJRF.Ventanas.GestionDatosGenericos.Proveedores;
using GestorJRF.Ventanas.GestionDatosGenericos.Resumenes;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using GestorJRF.Ventanas.GestionGastos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GestorJRF.Ventanas.GestionDatosGenericos
{
    /// <summary>
    /// Lógica de interacción para VentanaBusqueda.xaml
    /// </summary>
    public partial class VentanaBusqueda : Window
    {
        private VistaCamiones vCamionPadre;
        private VistaEmpresas vEmpresaPadre;
        private VistaEmpleados vEmpleadoPadre;
        private VistaTarifas vTarifaPadre;
        private VistaAlertas vAlertaPadre;
        private VentanaGestionResumenesPrevio vGestionResumenesPrevios;
        private VentanaGestionResumenesFinal vGestionResumenesFinal;
        private VistaGastos vGastoPadre;
        private VistaProveedores vProveedoresPadre;

        private string tipo;

        public VentanaBusqueda(Window ventanaPadre, string tipoBusqueda)
        {
            InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            tipo = tipoBusqueda;

            switch (tipo)
            {
                case "camion":
                    vCamionPadre = (VistaCamiones)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "NÚMERO DE MATRÍCULA");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "NÚMERO DE BASTIDOR");
                    break;
                case "empleado":
                    vEmpleadoPadre = (VistaEmpleados)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "DNI");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "APELLIDOS, NOMBRE");
                    break;
                case "empresa":
                    vEmpresaPadre = (VistaEmpresas)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "NOMBRE");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "CIF/NIF");
                    break;
                case "tarifa":
                    vTarifaPadre = (VistaTarifas)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "TIPO DE TARIFA [CLIENTE/GENERAL]");
                    break;
                case "alerta":
                    lTipoAlerta.Visibility = Visibility.Visible;
                    cAlerta.Visibility = Visibility.Visible;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "ID");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "DESCRIPCIÓN");
                    cAlerta.SelectedIndex = 0;
                    tipo = tipo + "FECHA";
                    vAlertaPadre = (VistaAlertas)ventanaPadre;
                    break;
                case "resumen previo":
                    vGestionResumenesPrevios = (VentanaGestionResumenesPrevio)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "ID");
                    break;
                case "resumen final":
                    vGestionResumenesFinal = (VentanaGestionResumenesFinal)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "ID");
                    break;
                case "gasto":
                    vGastoPadre = (VistaGastos)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "ID");
                    break;
                case "proveedor":
                    vProveedoresPadre = (VistaProveedores)ventanaPadre;
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "CIF");
                    cBusqueda.Items.Add(new ComboBoxItem().Content = "NOMBRE");
                    break;
                default:
                    break;
            }
            cBusqueda.SelectedIndex = 0;
        }

        private void bListadoCompleto_Click(object sender, RoutedEventArgs e)
        {
            if (tipo.Contains("resumen") || tipo.Equals("tarifa") || tipo.Equals("empresa"))
                new VistaCompletaDosTablas(tipo).Show();
            else
                new VistaCompletaTabla(tipo).Show();
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (!tBusqueda.Text.Equals(""))
            {
                Resumen resumen;
                switch (tipo)
                {
                    case "camion":
                        Camion camion;
                        if (cBusqueda.SelectedValue.Equals("NÚMERO DE MATRÍCULA"))
                            camion = CamionesCRUD.cogerCamion("NÚMERO DE MATRÍCULA", tBusqueda.Text);
                        else
                            camion = CamionesCRUD.cogerCamion("NÚMERO DE BASTIDOR", tBusqueda.Text);

                        if (camion != null)
                        {
                            vCamionPadre.camion = camion;
                            vCamionPadre.MostrarCamionBuscado();
                            Close();
                        }
                        else
                            MessageBox.Show("El camión buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case "empleado":
                        Empleado empleado;
                        if (cBusqueda.SelectedValue.Equals("DNI"))
                            empleado = EmpleadosCRUD.cogerEmpleado("DNI", tBusqueda.Text);
                        else
                            empleado = EmpleadosCRUD.cogerEmpleado("NOMBRE", tBusqueda.Text);

                        if (empleado != null)
                        {
                            vEmpleadoPadre.empleado = empleado;
                            vEmpleadoPadre.MostrarEmpleadoBuscado();
                            Close();
                        }
                        else
                            MessageBox.Show("El camión buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case "empresa":
                        Empresa empresa;
                        if (cBusqueda.SelectedValue.Equals("NOMBRE"))
                            empresa = EmpresasCRUD.cogerEmpresa("nombre", tBusqueda.Text);
                        else
                            empresa = EmpresasCRUD.cogerEmpresa("cif", tBusqueda.Text);

                        if (empresa != null)
                        {
                            vEmpresaPadre.empresa = empresa;
                            vEmpresaPadre.MostrarEmpresaBuscada();
                            Close();
                        }                            

                        break;

                    case "tarifa":
                        Tarifa tarifa = TarifasCRUD.cogerTarifaPorNombreTarifa(tBusqueda.Text);
                        if (tarifa != null)
                        {
                            vTarifaPadre.tarifa = tarifa;
                            vTarifaPadre.MostrarTarifaBuscada();
                            Close();
                        }
                        else
                            MessageBox.Show("La tarifa buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case "alertaFECHA":
                        Alerta alertaFecha;
                        if (cBusqueda.SelectedValue.Equals("ID"))
                            alertaFecha = AlertasCRUD.cogerAlertaFecha("id", tBusqueda.Text);
                        else
                            alertaFecha = AlertasCRUD.cogerAlertaFecha("descripcion", tBusqueda.Text);

                        if (alertaFecha != null)
                        {
                            vAlertaPadre.alerta = alertaFecha;
                            vAlertaPadre.MostrarAlertaBuscada();
                            Close();
                        }
                        else
                            MessageBox.Show("La alerta buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "alertaKILOMETRAJE":
                        Alerta alertaKM;
                        if (cBusqueda.SelectedValue.Equals("ID"))
                            alertaKM = AlertasCRUD.cogerAlertaKM("id", tBusqueda.Text);
                        else
                            alertaKM = AlertasCRUD.cogerAlertaKM("descripcion", tBusqueda.Text);

                        if (alertaKM != null)
                        {
                            vAlertaPadre.alerta = alertaKM;
                            vAlertaPadre.MostrarAlertaBuscada();
                            Close();
                        }
                        else
                            MessageBox.Show("La alerta buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case "resumen previo":
                        resumen = ResumenesCRUD.cogerResumenPrevio(Convert.ToInt64(tBusqueda.Text));
                        if (resumen != null)
                        {
                            vGestionResumenesPrevios.resumen = resumen;
                            vGestionResumenesPrevios.MostrarResumenBuscado();
                            Close();
                        }
                        else
                            MessageBox.Show("La tarifa buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case "resumen final":
                        resumen = ResumenesCRUD.cogerResumenFinal(Convert.ToInt64(tBusqueda.Text));
                        if (resumen != null)
                        {
                            vGestionResumenesFinal.resumen = resumen;
                            vGestionResumenesFinal.MostrarResumenBuscado();
                            Close();
                        }
                        else
                            MessageBox.Show("La tarifa buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case "gasto":
                        Gasto gasto = GastosCRUD.cogerGasto(Convert.ToInt64(tBusqueda.Text));
                        if (gasto != null)
                        {
                            vGastoPadre.gasto = gasto;
                            vGastoPadre.MostrarGastoBuscado();
                            Close();
                        }
                        else
                            MessageBox.Show("El gasto buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case "proveedor":
                        string tipo;
                        if (cBusqueda.SelectedIndex == 0)
                            tipo = "cif";
                        else
                            tipo = "nombre";
                        Proveedor proveedor = ProveedoresCRUD.cogerProveedor(tipo, tBusqueda.Text);
                        if (proveedor != null)
                        {
                            vProveedoresPadre.proveedor = proveedor;
                            vProveedoresPadre.MostrarProveedorBuscado();
                            Close();
                        }
                        else
                            MessageBox.Show("El proveedor buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    default:
                        break;
                }
            }
            else
                MessageBox.Show("El campo de búsqueda está vacío.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void cAlerta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cAlerta.IsVisible)
                tipo = "alerta" + ((ComboBoxItem)cAlerta.SelectedItem).Content;
        }
    }
}
