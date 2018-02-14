using GestorJRF.CRUD;
using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using GestorJRF.POJOS.Facturas;
using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace GestorJRF.CRUD
{
    internal class FacturasCRUD
    {
        internal static int añadirFactura(Factura f)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarFactura", f);
                FacturasCRUD.añadirComponenteFactura(f);
                MessageBox.Show("Factura almacenada correctamente en la BBDD.", "Nuevo factura", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("La factura introducida ya está almacenada.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación de la nueva factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        private static void añadirComponenteFactura(Factura f)
        {
            try
            {
                foreach (ComponenteFactura resumene in f.resumenes)
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarComponenteFactura", resumene);
                }
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("Algún componente de la factura está repetido.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación de los componentes de la factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                Trace.WriteLine(ex.ToString());
                throw ex;
            }
        }

        internal static Factura cogerFacturaPorNumero(long numeroFactura)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Factura factura = (Factura)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerFacturaPorNumero", numeroFactura);
                if (factura != null)
                {
                    factura.resumenes = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesFactura", numeroFactura).Cast<ComponenteFactura>().ToList();
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return factura;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger la factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasFacturasPorFechas(BusquedaFactura opcionesBusqueda)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList facturas = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerFacturaPorFechas", opcionesBusqueda);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return facturas;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger la factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static Factura cogerFacturaPorIdResumen(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Factura factura = (Factura)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerFacturaPorIdResumen", id);
                if (factura != null)
                {
                    factura.resumenes = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesFactura", factura.numeroFactura).Cast<ComponenteFactura>().ToList();
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return factura;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger la factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static Factura cogerFacturaPorNumeroParaAbono(Factura f)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Factura factura = (Factura)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerFacturaPorNumeroParaAbono", f);
                if (factura != null)
                {
                    factura.resumenes = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesFactura", f.numeroFactura).Cast<ComponenteFactura>().ToList();
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return factura;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger la factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasFacturas()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasFacturas", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todas las facturas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosComponentes(long numeroFactura)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosComponentesFactura", numeroFactura);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todas los componentes de factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static List<int> comprobarResumenesFacturados(List<Resumen> resumenes)
        {
            List<int> resumenesFacturados = new List<int>();
            int contador = 1;
            foreach (Resumen resumene in resumenes)
            {
                try
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                    IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerComponentesFactura", resumene.idAntiguo);
                    if (lista.Count > 0)
                    {
                        resumenesFacturados.Add(contador);
                    }
                    contador++;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show("Error al coger algún componente de factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                    contador++;
                    Trace.WriteLine(ex.ToString());
                }
            }
            return resumenesFacturados;
        }
    }

}
