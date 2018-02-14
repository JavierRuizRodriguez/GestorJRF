using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS.Pagos;
using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace GestorJRF.CRUD
{
    internal class PagosCRUD
    {
        internal static int añadirPago(Pago p)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                long id = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarPago", p);
                foreach (ComponentePago factura in p.facturas)
                {
                    factura.idPago = id;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarComponentePago", factura);
                }
                MessageBox.Show("Pago almacenado correctamente en la BBDD.", "Nuevo pago", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("Una de las facturas ya esta asociada a este pago.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo pago.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static IList cogerTodosPagos()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                List<Pago> lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosPagos", null).Cast<Pago>().ToList();
                if (lista != null)
                {
                    foreach (Pago item in lista)
                    {
                        item.facturas = new List<ComponentePago>(InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerComponentesPagoPorId", item.id).Cast<ComponentePago>().ToList());
                    }
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los pagos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosPagareSinBanco()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                List<Pago> lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosPagareSinBanco", null).Cast<Pago>().ToList();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los pagarés sin pago.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosComponentesPagoPorId(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerComponentesPagoPorId", id);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los pagarés sin pago.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int modificarPago(Pago p)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarPago", p.id);
                long id = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarPago", p);
                foreach (ComponentePago factura in p.facturas)
                {
                    factura.idPago = id;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarComponentePago", factura);
                }
                MessageBox.Show("Pago modificado correctamente en la BBDD.", "Pago actualizado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error en la actualización del pago.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static int borrarPago(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarPago", id);
                MessageBox.Show("Pago borrado correctamente en la BBDD.", "Pago borrado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error en el borrado del pago.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static Pago cogerPago(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Pago pago = (Pago)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerPagoPorId", id);
                if (pago != null)
                {
                    pago.facturas = new List<ComponentePago>(InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerComponentesPagoPorId", pago.id).Cast<ComponentePago>().ToList());
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return pago;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al buscar el pago.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
