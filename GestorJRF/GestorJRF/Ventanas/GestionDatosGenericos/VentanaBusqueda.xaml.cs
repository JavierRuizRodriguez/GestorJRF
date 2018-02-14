using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas.Genericas;
using GestorJRF.Ventanas.GestionDatosGenericos.Alertas;
using GestorJRF.Ventanas.GestionDatosGenericos.Camiones;
using GestorJRF.Ventanas.GestionDatosGenericos.Empleados;
using GestorJRF.Ventanas.GestionDatosGenericos.Empresas;
using GestorJRF.Ventanas.GestionDatosGenericos.Gastos;
using GestorJRF.Ventanas.GestionDatosGenericos.Proveedores;
using GestorJRF.Ventanas.GestionDatosGenericos.Resumenes;
using GestorJRF.Ventanas.GestionDatosGenericos.Tarifas;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

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
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.tipo = tipoBusqueda;
            switch (this.tipo)
            {
                case "camion":
                    {
                        this.vCamionPadre = (VistaCamiones)ventanaPadre;
                        ItemCollection items13 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem13 = new ComboBoxItem();
                        object newItem = comboBoxItem13.Content = "NÚMERO DE MATRÍCULA";
                        items13.Add(newItem);
                        ItemCollection items14 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem14 = new ComboBoxItem();
                        newItem = (comboBoxItem14.Content = "NÚMERO DE BASTIDOR");
                        items14.Add(newItem);
                        break;
                    }
                case "empleado":
                    {
                        this.vEmpleadoPadre = (VistaEmpleados)ventanaPadre;
                        ItemCollection items11 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem11 = new ComboBoxItem();
                        object newItem = comboBoxItem11.Content = "DNI";
                        items11.Add(newItem);
                        ItemCollection items12 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem12 = new ComboBoxItem();
                        newItem = (comboBoxItem12.Content = "APELLIDOS, NOMBRE");
                        items12.Add(newItem);
                        break;
                    }
                case "empresa":
                    {
                        this.vEmpresaPadre = (VistaEmpresas)ventanaPadre;
                        ItemCollection items9 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem9 = new ComboBoxItem();
                        object newItem = comboBoxItem9.Content = "NOMBRE";
                        items9.Add(newItem);
                        ItemCollection items10 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem10 = new ComboBoxItem();
                        newItem = (comboBoxItem10.Content = "CIF/NIF");
                        items10.Add(newItem);
                        break;
                    }
                case "tarifa":
                    {
                        this.vTarifaPadre = (VistaTarifas)ventanaPadre;
                        ItemCollection items8 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem8 = new ComboBoxItem();
                        object newItem = comboBoxItem8.Content = "TIPO DE TARIFA [CLIENTE/GENERAL]";
                        items8.Add(newItem);
                        break;
                    }
                case "alerta":
                    {
                        this.lTipoAlerta.Visibility = Visibility.Visible;
                        this.cAlerta.Visibility = Visibility.Visible;
                        ItemCollection items6 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem6 = new ComboBoxItem();
                        object newItem = comboBoxItem6.Content = "ID";
                        items6.Add(newItem);
                        ItemCollection items7 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem7 = new ComboBoxItem();
                        newItem = (comboBoxItem7.Content = "DESCRIPCIÓN");
                        items7.Add(newItem);
                        this.cAlerta.SelectedIndex = -1;
                        this.tipo += "FECHA";
                        this.vAlertaPadre = (VistaAlertas)ventanaPadre;
                        break;
                    }
                case "resumen previo":
                    {
                        this.vGestionResumenesPrevios = (VentanaGestionResumenesPrevio)ventanaPadre;
                        ItemCollection items5 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem5 = new ComboBoxItem();
                        object newItem = comboBoxItem5.Content = "ID";
                        items5.Add(newItem);
                        break;
                    }
                case "resumen final":
                    {
                        this.vGestionResumenesFinal = (VentanaGestionResumenesFinal)ventanaPadre;
                        ItemCollection items4 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem4 = new ComboBoxItem();
                        object newItem = comboBoxItem4.Content = "ID";
                        items4.Add(newItem);
                        break;
                    }
                case "gasto":
                    {
                        this.lTipoGasto.Visibility = Visibility.Visible;
                        this.cGasto.Visibility = Visibility.Visible;
                        this.vGastoPadre = (VistaGastos)ventanaPadre;
                        ItemCollection items3 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem3 = new ComboBoxItem();
                        object newItem = comboBoxItem3.Content = "ID";
                        items3.Add(newItem);
                        break;
                    }
                case "proveedor":
                    {
                        this.vProveedoresPadre = (VistaProveedores)ventanaPadre;
                        ItemCollection items = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        object newItem = comboBoxItem.Content = "CIF";
                        items.Add(newItem);
                        ItemCollection items2 = this.cBusqueda.Items;
                        ComboBoxItem comboBoxItem2 = new ComboBoxItem();
                        newItem = (comboBoxItem2.Content = "NOMBRE");
                        items2.Add(newItem);
                        break;
                    }
            }
            this.cBusqueda.SelectedIndex = -1;
        }

        private void bListadoCompleto_Click(object sender, RoutedEventArgs e)
        {
            if (this.tipo.Contains("resumen") || this.tipo.Equals("tarifa") || this.tipo.Equals("empresa"))
            {
                new VistaCompletaDosTablas(this.tipo).Show();
            }
            else
            {
                new VistaCompletaTabla(this.tipo).Show();
            }
        }

        private void bBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (!this.tBusqueda.Text.Equals(""))
            {
                switch (this.tipo)
                {
                    case "camion":
                        {
                            Camion camion = (!this.cBusqueda.SelectedValue.Equals("NÚMERO DE MATRÍCULA")) ? CamionesCRUD.cogerCamion("NÚMERO DE BASTIDOR", this.tBusqueda.Text) : CamionesCRUD.cogerCamion("NÚMERO DE MATRÍCULA", this.tBusqueda.Text);
                            if (camion != null)
                            {
                                this.vCamionPadre.camion = camion;
                                this.vCamionPadre.MostrarCamionBuscado();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("El camión buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                    case "empleado":
                        {
                            Empleado empleado = (!this.cBusqueda.SelectedValue.Equals("DNI")) ? EmpleadosCRUD.cogerEmpleado("NOMBRE", this.tBusqueda.Text) : EmpleadosCRUD.cogerEmpleado("DNI", this.tBusqueda.Text);
                            if (empleado != null)
                            {
                                this.vEmpleadoPadre.empleado = empleado;
                                this.vEmpleadoPadre.MostrarEmpleadoBuscado();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("El empleado buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                    case "empresa":
                        {
                            Empresa empresa = (!this.cBusqueda.SelectedValue.Equals("NOMBRE")) ? EmpresasCRUD.cogerEmpresa("cif", this.tBusqueda.Text) : EmpresasCRUD.cogerEmpresa("nombre", this.tBusqueda.Text);
                            if (empresa != null)
                            {
                                this.vEmpresaPadre.empresa = empresa;
                                this.vEmpresaPadre.MostrarEmpresaBuscada();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("La empresa buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                    case "tarifa":
                        {
                            Tarifa tarifa = TarifasCRUD.cogerTarifaPorNombreTarifa(this.tBusqueda.Text);
                            if (tarifa != null)
                            {
                                this.vTarifaPadre.tarifa = tarifa;
                                this.vTarifaPadre.MostrarTarifaBuscada();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("La tarifa buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                    case "alertaFECHA":
                        {
                            Alerta alertaFecha = null;
                            if (this.cBusqueda.SelectedValue != null && this.cBusqueda.SelectedValue.Equals("ID"))
                            {
                                if (UtilidadesVerificacion.validadorNumeroEntero(this.tBusqueda.Text))
                                {
                                    alertaFecha = AlertasCRUD.cogerAlertaFecha("id", this.tBusqueda.Text);
                                }
                            }
                            else
                            {
                                alertaFecha = AlertasCRUD.cogerAlertaFecha("descripcion", this.tBusqueda.Text);
                            }
                            if (alertaFecha != null)
                            {
                                this.vAlertaPadre.alerta = alertaFecha;
                                this.vAlertaPadre.MostrarAlertaBuscada();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("La alerta buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                    case "alertaKILOMETRAJE":
                        {
                            Alerta alertaKM = null;
                            if (this.cBusqueda.SelectedValue != null && this.cBusqueda.SelectedValue.Equals("ID"))
                            {
                                if (UtilidadesVerificacion.validadorNumeroEntero(this.tBusqueda.Text))
                                {
                                    alertaKM = AlertasCRUD.cogerAlertaKM("id", this.tBusqueda.Text);
                                }
                            }
                            else
                            {
                                alertaKM = AlertasCRUD.cogerAlertaKM("descripcion", this.tBusqueda.Text);
                            }
                            if (alertaKM != null)
                            {
                                this.vAlertaPadre.alerta = alertaKM;
                                this.vAlertaPadre.MostrarAlertaBuscada();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("La alerta buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                    case "resumen previo":
                        {
                            Resumen resumen = null;
                            if (UtilidadesVerificacion.validadorNumeroEntero(this.tBusqueda.Text))
                            {
                                resumen = ResumenesCRUD.cogerResumenPrevio(Convert.ToInt64(this.tBusqueda.Text));
                                if (resumen != null)
                                {
                                    this.vGestionResumenesPrevios.resumen = resumen;
                                    this.vGestionResumenesPrevios.MostrarResumenBuscado();
                                    base.Close();
                                }
                                else
                                {
                                    MessageBox.Show("El resumen previo buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                                }
                            }
                            break;
                        }
                    case "resumen final":
                        if (UtilidadesVerificacion.validadorNumeroEntero(this.tBusqueda.Text))
                        {
                            Resumen resumen = ResumenesCRUD.cogerResumenFinal(Convert.ToInt64(this.tBusqueda.Text));
                            if (resumen != null)
                            {
                                this.vGestionResumenesFinal.resumen = resumen;
                                this.vGestionResumenesFinal.MostrarResumenBuscado();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("El resumen final buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                        }
                        break;
                    case "gastoNORMAL":
                        {
                            GastoNormal gastoNormal = (GastoNormal)GastosCRUD.cogerGasto(Convert.ToInt64(this.tBusqueda.Text), "normal");
                            if (gastoNormal != null)
                            {
                                this.vGastoPadre.setGasto(gastoNormal);
                                this.vGastoPadre.MostrarGastoBuscado();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("El gasto buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                    case "gastoBIEN INVERSIÓN":
                        {
                            GastoBienInversion gastoBienInversion = (GastoBienInversion)GastosCRUD.cogerGasto(Convert.ToInt64(this.tBusqueda.Text), "bien");
                            if (gastoBienInversion != null)
                            {
                                this.vGastoPadre.setGasto(gastoBienInversion);
                                this.vGastoPadre.MostrarGastoBuscado();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("El gasto buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                    case "proveedor":
                        {
                            string tipo = (this.cBusqueda.SelectedIndex != 0) ? "nombre" : "cif";
                            Proveedor proveedor = ProveedoresCRUD.cogerProveedor(tipo, this.tBusqueda.Text);
                            if (proveedor != null)
                            {
                                this.vProveedoresPadre.proveedor = proveedor;
                                this.vProveedoresPadre.MostrarProveedorBuscado();
                                base.Close();
                            }
                            else
                            {
                                MessageBox.Show("El proveedor buscado no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("El campo de búsqueda está vacío.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void cAlerta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cAlerta.IsVisible)
            {
                this.tipo = "alerta" + ((ComboBoxItem)this.cAlerta.SelectedItem).Content;
            }
        }

        private void cGasto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cGasto.IsVisible)
            {
                this.tipo = "gasto" + ((ComboBoxItem)this.cGasto.SelectedItem).Content;
            }
        }
    }
}
