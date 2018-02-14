using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionDatosGenericos;
using GestorJRF.Ventanas.GestionDatosGenericos.Gastos;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionDatosGenericos.Gastos
{
    /// <summary>
    /// Lógica de interacción para VistaGastos.xaml
    /// </summary>
    public partial class VistaGastos : Window
    {
        private Gasto gasto;

        public VistaGastos()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
        }

        public void setGasto(Gasto g)
        {
            this.gasto = g;
            this.lImporte.Content = ((g is GastoBienInversion) ? "Amortización" : this.lImporte.Content);
            this.lIrpf.Content = ((g is GastoBienInversion) ? "Intereses" : this.lIrpf.Content);
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            new VentanaMenuGestionDatos().Show();
        }

        private void bModificarGasto_Click(object sender, RoutedEventArgs e)
        {
            new VentanaGestionGastos(this.gasto).Show();
            UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
            this.gasto = null;
        }

        private void bBorrarGasto_Click(object sender, RoutedEventArgs e)
        {
            if (this.gasto != null)
            {
                if (MessageBox.Show("¿Desea borrar el gasto?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes && GastosCRUD.borrarGasto(this.gasto.id) == 1)
                {
                    UtilidadesVentana.LimpiarCampos(this.gridPrincipal);
                    this.gasto = null;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un gasto para borrarlo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void bBuscarGasto_Click(object sender, RoutedEventArgs e)
        {
            new VentanaBusqueda(this, "gasto").Show();
        }

        internal void MostrarGastoBuscado()
        {
            if (this.gasto != null)
            {
                if (this.gasto.dni == null)
                {
                    this.checkGastoGenerico.IsChecked = true;
                }
                else
                {
                    this.tEmpleado.Text = this.buscarNombreApellidosEmpleado();
                }
                this.tConcepto.Text = this.gasto.concepto;
                TextBox textBox = this.tFecha;
                DateTime dateTime = this.gasto.fecha;
                dateTime = dateTime.Date;
                textBox.Text = dateTime.ToString("dd/MM/yyyy");
                this.tDescripcion.Text = this.gasto.descripcion;
                this.tTrimestre.Text = this.gasto.numeroTrimestre.ToString();
                this.tIva.Text = Convert.ToString(this.gasto.iva);
                this.tReferencia.Text = this.gasto.referencia;
                TextBox textBox2 = this.tFecha;
                dateTime = this.gasto.fecha;
                dateTime = dateTime.Date;
                textBox2.Text = dateTime.ToString("dd/MM/yyyy");
                this.tProveedor.Text = this.gasto.proveedor;
                this.tTasas.Text = this.gasto.tasas.ToString("f2");
                this.tAño.Text = this.gasto.año.ToString();
                if (this.gasto is GastoNormal)
                {
                    this.tIrpf.Text = Convert.ToString(((GastoNormal)this.gasto).irpf);
                    if (this.gasto.concepto.Equals("MANUTENCIÓN"))
                    {
                        this.tImporte.Text = Convert.ToString(this.gasto.cuotaDeducible);
                        this.lImporte.Content = "Cuota deducible";
                    }
                    else
                    {
                        this.tImporte.Text = Convert.ToString(((GastoNormal)this.gasto).importeBase);
                    }
                }
                else
                {
                    this.tIrpf.Text = Convert.ToString(((GastoBienInversion)this.gasto).intereses);
                    this.tImporte.Text = Convert.ToString(((GastoBienInversion)this.gasto).amortizacion);
                }
            }
        }

        private string buscarNombreApellidosEmpleado()
        {
            Empleado empleado = EmpleadosCRUD.cogerEmpleado("DNI", this.gasto.dni);
            if (empleado != null)
            {
                return empleado.getNombreApellidos();
            }
            return "NO ENCONTRADO";
        }
    }
}
