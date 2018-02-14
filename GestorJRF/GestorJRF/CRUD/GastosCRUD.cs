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
    class GastosCRUD
    {
        internal static int añadirGasto(Gasto gasto)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                if (gasto is GastoNormal)
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarGastoNormal", (GastoNormal)gasto);
                }
                else
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarGastoBienInversion", (GastoBienInversion)gasto);
                }
                MessageBox.Show("Gasto almacenado correctamente en la BBDD.", "Nuevo gasto", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El gasto introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static Gasto cogerGasto(long id, string tipo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Gasto gasto = (!tipo.Equals("normal")) ? ((Gasto)(GastoBienInversion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerGastoBienInversionPorId", id)) : ((Gasto)(GastoNormal)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerGastoNormalPorId", id));
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return gasto;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al buscar el gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosParaEstadistica(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerGastosNormalesParaEstadisticas", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosNormal()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosGastosNormal", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosBienInversion()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosGastosBienInversion", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosParaEstadisticaPorProveedor(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerGastosNormalesParaEstadisticasPorProveedor", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosParaEstadisticaPorEmpleado(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerGastosNormalesParaEstadisticasPorEmpleado", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static double cogerSumatorioGastosParaEstadisticaPorProveedor(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                object sumatorio = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerSumatorioGastosNormalesParaEstadisticasPorProveedor", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                if (sumatorio != null)
                {
                    return Convert.ToDouble(sumatorio);
                }
                return 0.0;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return 0.0;
            }
        }

        internal static double cogerSumatorioGastosParaEstadisticaPorEmpleado(BusquedaEstadisticas opciones)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                object sumatorio = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerSumatorioGastosNormalesParaEstadisticasPorEmpleado", opciones);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                if (sumatorio != null)
                {
                    return Convert.ToDouble(sumatorio);
                }
                return 0.0;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return 0.0;
            }
        }

        internal static IList cogerTodosGastosNormalesPorFecha(Fechas fecha)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosGastosNormalPorFechas", fecha);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos por fechas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosBienInversionPorFecha(Fechas fecha)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosGastosBienInversionPorFechas", fecha);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos por fechas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosNormalesPorTrimestreAño(TrimestreAño x)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosGastosNormalPorTrimestreAño", x);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos por fechas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosGastosBienInversionPorTrimestreAño(TrimestreAño x)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosGastosBienInversionPorTrimestreAño", x);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los gastos por fechas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                MessageBox.Show("Gasto borrado correctamente en la BBDD.", "Gasto borrado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la eliminación del gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return -1;
            }
        }

        internal static int modificarGastoNormal(GastoNormal gasto)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarGastoNormal", gasto);
                MessageBox.Show("Gasto modificado correctamente en la BBDD.", "Gasto actualizado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación del gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return -1;
            }
        }

        internal static int modificarGastoBienInversion(GastoBienInversion gasto)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarGastoBienInversion", gasto);
                MessageBox.Show("Gasto modificado correctamente en la BBDD.", "Gasto actualizado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación del gasto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return -1;
            }
        }
    }
}
