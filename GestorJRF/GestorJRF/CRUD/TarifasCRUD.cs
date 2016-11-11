using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using Npgsql;
using System.Diagnostics;
using System.Windows;
using System.Collections;
using System.Linq;

namespace GestorJRF.CRUD
{
    class TarifasCRUD
    {
        internal static int añadirTarifa(Tarifa t)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarTarifa", t);
                añadirComponenteTarifa(t);
                MessageBox.Show("Tarifa almacenada correctamente.", "Nuevo tarifa", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("La tarifa para la empresa seleccionada ya está almacenada.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación de la nueva tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        private static void añadirComponenteTarifa(Tarifa tarifa)
        {
            try
            {
                foreach (ComponenteTarifa componente in tarifa.listaComponentesTarifa)
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarComponenteTarifa",
                        new ComponenteTarifa(tarifa.nombreTarifa, componente.etiqueta, componente.precio, componente.tipoCamion));
                }
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState.Equals("23505"))
                    MessageBox.Show("Algún componente de la tarifa está repetido.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error en la creación de los componentes de las tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                throw ex;
            }
        }

        internal static Tarifa cogerTarifaPorNombreTarifa(string nombreTarifa)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Tarifa tarifa = (Tarifa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerTarifaPorNombreTarifa", nombreTarifa);
                if (tarifa != null)
                    tarifa.listaComponentesTarifa = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesTarifas", tarifa.nombreTarifa).Cast<ComponenteTarifa>().ToList();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return tarifa;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger la tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static Tarifa cogerTarifaPorNombreEmpresa(string nombreEmpresa)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Tarifa tarifa = (Tarifa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerTarifaPorNombreEmpresa", nombreEmpresa);
                if (tarifa != null)
                    tarifa.listaComponentesTarifa = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesTarifas", tarifa.nombreTarifa).Cast<ComponenteTarifa>().ToList();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return tarifa;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger la tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasTarifas()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasTarifas", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todas las tarifas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosComponentes(string nombreTarifa)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesTarifas", nombreTarifa);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todas los componentes de tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }
        internal static IList cogerTodosComponentes(DobleCadena cadenas)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesTarifasPorTipo", cadenas);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todas los componentes de tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static void modificarTarifa(Tarifa tarifa)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarComponentesTarifa", tarifa.nombreTarifaAntiguo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarTarifa", tarifa);
                foreach (ComponenteTarifa componente in tarifa.listaComponentesTarifa)
                {
                    componente.nombreTarifa = tarifa.nombreTarifa;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarComponenteTarifa", componente);
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Tarifa modificada correctamente.", "Tarifa actualizada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la Tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal static int borrarTarfia(string nombre_tarifa)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarTarifa", nombre_tarifa);
                MessageBox.Show("Tarifa borrada correctamente.", "Tarifa borrada", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }
    }
}
