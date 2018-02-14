using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using Npgsql;
using System.Diagnostics;
using System.Windows;
using System.Collections;
using System.Linq;

namespace GestorJRF.CRUD
{
    internal class TarifasCRUD
    {
        internal static int añadirTarifa(Tarifa t)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarTarifa", t);
                TarifasCRUD.añadirComponenteTarifa(t);
                MessageBox.Show("Tarifa almacenada correctamente en la BBDD.", "Nuevo tarifa", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("La tarifa para la empresa seleccionada ya está almacenada.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación de la nueva tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        private static void añadirComponenteTarifa(Tarifa tarifa)
        {
            try
            {
                foreach (ComponenteTarifa item in tarifa.listaComponentesTarifa)
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarComponenteTarifa", new ComponenteTarifa(tarifa.nombreTarifa, item.etiqueta, item.precio, item.tipoCamion));
                }
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("Algún componente de la tarifa está repetido.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación de los componentes de las tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                Trace.WriteLine(ex.ToString());
                throw ex;
            }
        }

        internal static Tarifa cogerTarifaPorNombreTarifa(string nombreTarifa)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Tarifa tarifa = (Tarifa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerTarifaPorNombreTarifa", "%" + nombreTarifa + "%");
                if (tarifa != null)
                {
                    tarifa.listaComponentesTarifa = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesTarifas", tarifa.nombreTarifa).Cast<ComponenteTarifa>().ToList();
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return tarifa;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger la tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                {
                    tarifa.listaComponentesTarifa = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesTarifas", tarifa.nombreTarifa).Cast<ComponenteTarifa>().ToList();
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return tarifa;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger la tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasTarifas", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todas las tarifas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesTarifas", nombreTarifa);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todas los componentes de tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesTarifasPorTipo", cadenas);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todas los componentes de tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int modificarTarifa(Tarifa tarifa)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarComponentesTarifa", tarifa.nombreTarifaAntiguo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarTarifa", tarifa);
                foreach (ComponenteTarifa item in tarifa.listaComponentesTarifa)
                {
                    item.nombreTarifa = tarifa.nombreTarifa;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarComponenteTarifa", item);
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Tarifa modificada correctamente en la BBDD.", "Tarifa actualizada", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la Tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return -1;
            }
        }

        internal static int borrarTarfia(string nombre_tarifa)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarTarifa", nombre_tarifa);
                MessageBox.Show("Tarifa borrada correctamente en la BBDD.", "Tarifa borrada", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la tarifa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return -1;
            }
        }
    }
}
