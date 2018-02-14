using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos.Gastos;
using GestorJRF.Ventanas.GestionDatosGenericos.Proveedores;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Gastos
{
    /// <summary>
    /// Lógica de interacción para VentanaGestionGastos.xaml
    /// </summary>
    public partial class VentanaGestionGastos : Window
    {
        private Gasto gasto;

        private bool esAlta;

        private List<Empleado> listaEmpleados;

        private List<Proveedor> listaProveedores;

        public VentanaGestionGastos()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = true;
            this.inicializarComboboxes();
            this.inicializarIndicesComboboxes();
            this.tAño.Content = DateTime.Now.Year.ToString();
        }

        public VentanaGestionGastos(Gasto gasto)
        {
            this.InitializeComponent();
            this.gasto = gasto;
            UtilidadesVentana.SituarVentana(0, this);
            this.esAlta = false;
            this.inicializarComboboxes();
            this.inicializarIndicesComboboxes();
            TextBox textBox = this.tFecha;
            DateTime dateTime = gasto.fecha;
            dateTime = dateTime.Date;
            textBox.Text = dateTime.ToString("dd/MM/yyyy");
            this.tDescripcion.Text = gasto.descripcion;
            this.tTrimestre.Text = gasto.numeroTrimestre.ToString();
            this.tIva.Text = Convert.ToString(gasto.iva);
            this.tReferencia.Text = gasto.referencia;
            TextBox textBox2 = this.tFecha;
            dateTime = gasto.fecha;
            dateTime = dateTime.Date;
            textBox2.Text = dateTime.ToString("dd/MM/yyyy");
            this.tTasas.Text = gasto.tasas.ToString("f2");
            this.tAño.Content = gasto.año.ToString();
            if (gasto is GastoNormal)
            {
                this.tIrpf.Text = Convert.ToString(((GastoNormal)gasto).irpf);
                if (gasto.concepto.Equals("MANUTENCIÓN"))
                {
                    this.tImporte.Text = Convert.ToString(gasto.cuotaDeducible);
                    this.lImporte.Content = "Cuota deducible";
                }
                else
                {
                    this.tImporte.Text = Convert.ToString(((GastoNormal)gasto).importeBase);
                }
            }
            else
            {
                this.tIrpf.Text = Convert.ToString(((GastoBienInversion)gasto).intereses);
                this.tImporte.Text = Convert.ToString(((GastoBienInversion)gasto).amortizacion);
            }
            this.tImporte.Text = this.tImporte.Text.Replace(",", ".");
            this.tTasas.Text = this.tTasas.Text.Replace(",", ".");
            this.tIrpf.Text = this.tIrpf.Text.Replace(",", ".");
            this.bNuevoGasto.Content = "MODIFICAR GASTO";
        }

        private void inicializarIndicesComboboxes()
        {
            if (this.esAlta)
            {
                this.reinicarComboBoxes();
            }
            else
            {
                if (this.gasto is GastoBienInversion)
                {
                    this.checkBienInversion.IsChecked = true;
                }
                else if (this.gasto.dni == null)
                {
                    this.checkGastoGenerico.IsChecked = true;
                }
                else
                {
                    this.cEmpleado.SelectedIndex = this.buscarIndiceEmpleado();
                }
                switch (this.gasto.concepto)
                {
                    case "COMBUSTIBLE":
                        this.cConcepto.SelectedIndex = 0;
                        break;
                    case "MANUTENCIÓN":
                        this.cConcepto.SelectedIndex = 1;
                        break;
                    case "MANTENIMIENTO":
                        this.cConcepto.SelectedIndex = 2;
                        break;
                    case "DOCUMENTACIÓN":
                        this.cConcepto.SelectedIndex = 3;
                        break;
                    case "TRANSPORTE":
                        this.cConcepto.SelectedIndex = 4;
                        break;
                    case "OTROS":
                        this.cConcepto.SelectedIndex = 5;
                        break;
                }
                this.cProveedor.SelectedIndex = this.buscarIndiceProveedor();
            }
        }

        private int buscarIndiceProveedor()
        {
            int contador = 0;
            foreach (Proveedor listaProveedore in this.listaProveedores)
            {
                if (listaProveedore.nombre.Equals(this.gasto.proveedor))
                {
                    return contador;
                }
                contador++;
            }
            return contador;
        }

        private int buscarIndiceEmpleado()
        {
            int contador = 0;
            foreach (Empleado listaEmpleado in this.listaEmpleados)
            {
                if (listaEmpleado.dni.Equals(this.gasto.dni))
                {
                    return contador;
                }
                contador++;
            }
            return contador;
        }

        private void inicializarComboboxes()
        {
            this.listaEmpleados = EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList();
            if (this.listaEmpleados != null)
            {
                this.listaEmpleados = new List<Empleado>(from e in this.listaEmpleados
                                                         orderby e.nombre
                                                         select e);
                foreach (Empleado listaEmpleado in this.listaEmpleados)
                {
                    ItemCollection items = this.cEmpleado.Items;
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    object newItem = comboBoxItem.Content = listaEmpleado.getNombreApellidos();
                    items.Add(newItem);
                }
            }
            this.listaProveedores = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToList();
            if (this.listaProveedores != null)
            {
                this.listaProveedores = new List<Proveedor>(from p in this.listaProveedores
                                                            orderby p.nombre
                                                            select p);
                foreach (Proveedor listaProveedore in this.listaProveedores)
                {
                    ItemCollection items2 = this.cProveedor.Items;
                    ComboBoxItem comboBoxItem2 = new ComboBoxItem();
                    object newItem = comboBoxItem2.Content = listaProveedore.nombre;
                    items2.Add(newItem);
                }
            }
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            if (this.esAlta)
            {
                new VentanaMenuGestionDatos().Show();
            }
        }

        private void checkGastoGenerico_Checked(object sender, RoutedEventArgs e)
        {
            this.cEmpleado.IsEnabled = false;
            this.checkBienInversion.IsEnabled = false;
            this.reinicarComboBoxes();
        }

        private void checkGastoGenerico_Unchecked(object sender, RoutedEventArgs e)
        {
            this.cEmpleado.IsEnabled = true;
            this.checkBienInversion.IsEnabled = true;
            this.reinicarComboBoxes();
        }

        private void bLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
        }

        private void bNuevoGasto_Click(object sender, RoutedEventArgs e)
        {
            if (VentanaGestionGastos.ComprobarCampos(this.gridPrincipal))
            {
                if (Convert.ToInt32(this.tTrimestre.Text) > 0 && Convert.ToInt32(this.tTrimestre.Text) < 5)
                {
                    if (UtilidadesVerificacion.validadorFechas(this.tFecha.Text) && UtilidadesVerificacion.validadorNumeroDecimal(this.tImporte.Text) && UtilidadesVerificacion.validadorNumeroEntero(this.tIva.Text) && UtilidadesVerificacion.validadorNumeroEntero(this.tAño.Content.ToString()) && UtilidadesVerificacion.validadorNumeroDecimal(this.tIrpf.Text) && UtilidadesVerificacion.validadorNumeroEntero(this.tTrimestre.Text))
                    {
                        if (this.esAlta)
                        {
                            this.añadirGasto();
                        }
                        else
                        {
                            this.modificarGasto();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El campo 'Trimestre' debe ser [1,4].", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                MessageBox.Show("Debe introducir todos los campos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void modificarGasto()
        {
            Gasto gasto = this.generarGasto();
            long salida = (this.checkBienInversion.IsChecked == true) ? GastosCRUD.modificarGastoBienInversion((GastoBienInversion)gasto) : GastosCRUD.modificarGastoNormal((GastoNormal)gasto);
            if (salida == 1)
            {
                base.Close();
            }
        }

        private Gasto generarGasto()
        {
            int iva = Convert.ToInt32(this.tIva.Text);
            double irpf = Convert.ToDouble(this.tIrpf.Text, UtilidadesVerificacion.cogerProveedorDecimal());
            double cuotaDeducible2;
            if (this.checkBienInversion.IsChecked == false)
            {
                double importeBase;
                if (this.cConcepto.SelectedIndex == 1)
                {
                    cuotaDeducible2 = Convert.ToDouble(this.tImporte.Text, UtilidadesVerificacion.cogerProveedorDecimal());
                    importeBase = cuotaDeducible2 * 100.0 / (double)(100 + iva);
                }
                else
                {
                    importeBase = Convert.ToDouble(this.tImporte.Text, UtilidadesVerificacion.cogerProveedorDecimal());
                    cuotaDeducible2 = importeBase + (importeBase * (double)iva / 100.0 - importeBase * irpf / 100.0 + Convert.ToDouble(this.tTasas.Text, UtilidadesVerificacion.cogerProveedorDecimal()));
                }
                string emplelado = (this.checkGastoGenerico.IsChecked == true) ? null : this.listaEmpleados[this.cEmpleado.SelectedIndex].dni;
                if (this.esAlta)
                {
                    return new GastoNormal(emplelado, ((ComboBoxItem)this.cConcepto.SelectedItem).Content.ToString(), Convert.ToDateTime(this.tFecha.Text), this.tDescripcion.Text, cuotaDeducible2, iva, this.tReferencia.Text, this.listaProveedores[this.cProveedor.SelectedIndex].nombre, this.listaProveedores[this.cProveedor.SelectedIndex].cif, Convert.ToDouble(this.tTasas.Text, UtilidadesVerificacion.cogerProveedorDecimal()), importeBase, irpf, Convert.ToInt32(this.tTrimestre.Text), DateTime.Now, Convert.ToInt32(this.tAño.Content));
                }
                return new GastoNormal(this.gasto.id, emplelado, ((ComboBoxItem)this.cConcepto.SelectedItem).Content.ToString(), Convert.ToDateTime(this.tFecha.Text), this.tDescripcion.Text, cuotaDeducible2, iva, this.tReferencia.Text, this.listaProveedores[this.cProveedor.SelectedIndex].nombre, this.listaProveedores[this.cProveedor.SelectedIndex].cif, Convert.ToDouble(this.tTasas.Text, UtilidadesVerificacion.cogerProveedorDecimal()), importeBase, irpf, Convert.ToInt32(this.tTrimestre.Text), DateTime.Now, Convert.ToInt32(this.tAño.Content));
            }
            double amortizacion = Convert.ToDouble(this.tImporte.Text, UtilidadesVerificacion.cogerProveedorDecimal());
            double intereses = irpf;
            cuotaDeducible2 = (amortizacion + intereses) * (double)iva / 100.0 + amortizacion + intereses + Convert.ToDouble(this.tTasas.Text, UtilidadesVerificacion.cogerProveedorDecimal());
            if (this.esAlta)
            {
                return new GastoBienInversion(this.listaEmpleados[this.cEmpleado.SelectedIndex].dni, ((ComboBoxItem)this.cConcepto.SelectedItem).Content.ToString(), Convert.ToDateTime(this.tFecha.Text), this.tDescripcion.Text, cuotaDeducible2, iva, this.tReferencia.Text, this.listaProveedores[this.cProveedor.SelectedIndex].nombre, this.listaProveedores[this.cProveedor.SelectedIndex].cif, Convert.ToDouble(this.tTasas.Text), amortizacion, intereses, Convert.ToInt32(this.tTrimestre.Text), DateTime.Now, Convert.ToInt32(this.tAño.Content));
            }
            return new GastoBienInversion(this.gasto.id, this.listaEmpleados[this.cEmpleado.SelectedIndex].dni, ((ComboBoxItem)this.cConcepto.SelectedItem).Content.ToString(), Convert.ToDateTime(this.tFecha.Text), this.tDescripcion.Text, cuotaDeducible2, iva, this.tReferencia.Text, this.listaProveedores[this.cProveedor.SelectedIndex].nombre, this.listaProveedores[this.cProveedor.SelectedIndex].cif, Convert.ToDouble(this.tTasas.Text), amortizacion, intereses, Convert.ToInt32(this.tTrimestre.Text), DateTime.Now, Convert.ToInt32(this.tAño.Content));
        }

        private void añadirGasto()
        {
            Gasto gasto = this.generarGasto();
            if (GastosCRUD.añadirGasto(gasto) == 1)
            {
                UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                this.inicializarIndicesComboboxes();
            }
        }

        private void cConcepto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.checkBienInversion.IsChecked == false)
            {
                if (this.cConcepto.SelectedIndex == 1)
                {
                    this.lImporte.Content = "Cuota deducible";
                }
                else
                {
                    this.lImporte.Content = "Importe base";
                }
            }
        }

        private void bNuevoProveedor_Click(object sender, RoutedEventArgs e)
        {
            new VentanaGestionProveedores(this).Show();
        }

        public void actualizarProveedores()
        {
            this.cProveedor.Items.Clear();
            this.listaProveedores = ProveedoresCRUD.cogerTodosProveedores().Cast<Proveedor>().ToList();
            if (this.listaProveedores != null)
            {
                this.listaProveedores = new List<Proveedor>(from p in this.listaProveedores
                                                            orderby p.nombre
                                                            select p);
                foreach (Proveedor listaProveedore in this.listaProveedores)
                {
                    ItemCollection items = this.cProveedor.Items;
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    object newItem = comboBoxItem.Content = listaProveedore.nombre;
                    items.Add(newItem);
                }
            }
            if (this.cProveedor.Items.Count > 0)
            {
                this.cProveedor.SelectedIndex = -1;
            }
        }

        internal static bool ComprobarCampos(Grid grid)
        {
            bool todosRellenos = true;
            foreach (UIElement child in grid.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    TextBox txt2 = (TextBox)child;
                    if (txt2.Name != "tDescripcion" && txt2.Text.Equals("") && txt2.IsEnabled)
                    {
                        todosRellenos = false;
                    }
                }
                else if (child.GetType() == typeof(PasswordBox))
                {
                    PasswordBox txt = (PasswordBox)child;
                    if (txt.Password.Equals("") && txt.IsEnabled)
                    {
                        todosRellenos = false;
                    }
                }
            }
            return todosRellenos;
        }

        private void checkBienInversion_Checked(object sender, RoutedEventArgs e)
        {
            this.checkGastoGenerico.IsEnabled = false;
            this.lImporte.Content = "Amortización";
            this.lIrpf.Content = "Intereses";
            this.reinicarComboBoxes();
            ComboBox comboBox = this.cEmpleado;
            ItemCollection items = this.cEmpleado.Items;
            ComboBoxItem comboBoxItem = new ComboBoxItem();
            object item = comboBoxItem.Content = "Javier Ruiz Fraile";
            comboBox.SelectedIndex = items.IndexOf(item);
            this.cEmpleado.IsEnabled = false;
        }

        private void checkBienInversion_Unchecked(object sender, RoutedEventArgs e)
        {
            this.checkGastoGenerico.IsEnabled = true;
            this.lImporte.Content = "Importe base";
            this.lIrpf.Content = "     IRPF";
            this.reinicarComboBoxes();
            this.cEmpleado.SelectedIndex = -1;
            this.cEmpleado.IsEnabled = true;
        }

        private void reinicarComboBoxes()
        {
            this.cProveedor.SelectedIndex = -1;
            this.cConcepto.SelectedIndex = -1;
            this.cEmpleado.SelectedIndex = -1;
        }

        private void bArriba_Click(object sender, RoutedEventArgs e)
        {

            tAño.Content = (int.Parse(tAño.Content.ToString()) + 1).ToString();
        }

        private void bAbajo_Click(object sender, RoutedEventArgs e)
        {

            tAño.Content = (int.Parse(tAño.Content.ToString()) - 1).ToString();
        }
    }
}
