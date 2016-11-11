using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Estadisticas;
using Npgsql;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;

namespace GestorJRF.CRUD
{
    public class GastosCRUD
    {
        internal static int añadirGasto(Gasto gasto)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarGasto", gasto);
                MessageBox.Show("Gasto almacenado correctamente.", "Nuevo gasto", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                    MessageBox.Show("El gasto introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error en la creación del nuevo gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static Gasto cogerGasto(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Gasto gasto = (Gasto)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerGastoPorId", id);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return gasto;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al buscar el gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastos()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosGastos", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosParaEstadistica(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerGastosParaEstadisticas", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosParaEstadisticaPorProveedor(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerGastosParaEstadisticasPorProveedor", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosParaEstadisticaPorEmpleado(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerGastosParaEstadisticasPorEmpleado", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static double cogerSumatorioGastosParaEstadisticaPorProveedor(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var sumatorio = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerSumatorioGastosParaEstadisticasPorProveedor", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                if (sumatorio != null)
                    return Convert.ToDouble(sumatorio);
                else
                    return 0;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return 0;
            }
        }

        internal static double cogerSumatorioGastosParaEstadisticaPorEmpleado(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var sumatorio = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerSumatorioGastosParaEstadisticasPorEmpleado", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                if (sumatorio != null)
                    return Convert.ToDouble(sumatorio);
                else
                    return 0;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return 0;
            }
        }

        internal static IList cogerTodosGastosPorFecha(Fechas fecha)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosGastosPorFechas", fecha);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos por fechas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int borrarGasto(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarGasto", id);
                MessageBox.Show("Gasto borrado correctamente.", "Gasto borrado", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la eliminación del gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        internal static int modificarGasto(Gasto gasto)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarGasto", gasto);
                MessageBox.Show("Gasto modificado correctamente.", "Gasto actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación del gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

    }
}
