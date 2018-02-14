using GestorJRF.CRUD;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Estadisticas;
using GestorJRF.POJOS.Facturas;
using GestorJRF.Utilidades;
using GestorJRF.Ventanas;
using GestorJRF.Ventanas.GestionInformes;
using GestorJRF.Ventanas.Login;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GestorJRF.Ventanas.GestionInformes
{
    /// <summary>
    /// Lógica de interacción para VentanaVisualizacionComisiones.xaml
    /// </summary>
    public partial class VentanaVisualizacionComisiones : Window
    {
        public List<KeyValuePair<string, int>> comisionesFijas;

        public List<Empleado> empleados;

        public VentanaVisualizacionComisiones()
        {
            this.InitializeComponent();
            UtilidadesVentana.SituarVentana(0, this);
            this.empleados = new List<Empleado>(EmpleadosCRUD.cogerTodosEmpleados().Cast<Empleado>().ToList());
            this.comisionesFijas = new List<KeyValuePair<string, int>>();
            foreach (Empleado empleado in this.empleados)
            {
                this.comisionesFijas.Add(new KeyValuePair<string, int>(empleado.getNombreApellidos(), empleado.comision));
            }
            this.tAño.Text = DateTime.Now.ToString("yyyy");
        }

        private void bCalculoComisiones_Click(object sender, RoutedEventArgs e)
        {
            DateTime fechaInicio = new DateTime(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1, 1);
            DateTime fechaFinal = new DateTime(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1, DateTime.DaysInMonth(Convert.ToInt32(this.tAño.Text), this.cMes.SelectedIndex + 1));
            BusquedaFactura opciones = new BusquedaFactura(fechaInicio, fechaFinal, "comisiones");
            List<ComisionesInforme> comisionesAux = new List<ComisionesInforme>(ResumenesCRUD.cogerComisionesParaInforme(opciones).Cast<ComisionesInforme>().ToList());
            List<ComisionesInforme> comisiones = new List<ComisionesInforme>();
            foreach (ComisionesInforme item in comisionesAux)
            {
                string nombre = (from emp in this.empleados
                                 where emp.dni == item.nombre
                                 select emp.getNombreApellidos()).FirstOrDefault();
                int porcentajeComision = (from par in this.comisionesFijas
                                          where par.Key.Equals(nombre)
                                          select par.Value).FirstOrDefault();
                comisiones.Add(new ComisionesInforme(nombre, item.importe * (double)porcentajeComision / 100.0));
            }
            this.tablaComisiones.ItemsSource = comisiones;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new VentanaMenuPrincipal().Show();
        }
    }
}
