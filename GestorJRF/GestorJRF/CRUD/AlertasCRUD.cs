using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using Npgsql;
using System.Diagnostics;
using System.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using IBatisNet.DataMapper.Exceptions;
using System.Linq;

namespace GestorJRF.CRUD
{
    class AlertasCRUD
    {
        internal static int añadirAlerta(Alerta alerta, int tipo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                if (tipo == 0)
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarAlertaFecha", alerta);
                }
                else
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarAlertaKM", alerta);
                }
                MessageBox.Show("Alerta almacenada correctamente en la BBDD.", "Nueva alerta", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("La alerta introducida ya está almacenada.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación de la nueva alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static List<AlertaKM> cogerTodasAlertasKMenKMs()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                List<AlertaKM> alertas = new List<AlertaKM>(InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerAlertasKmEnKms", null).Cast<AlertaKM>().ToList());
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return alertas;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar las alertas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static AlertaFecha cogerAlertaFecha(string tipo, string campo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                AlertaFecha alerta = (!tipo.Equals("id")) ? ((AlertaFecha)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAlertaFechaPorDescripcion", campo)) : ((AlertaFecha)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAlertaFechaPorId", Convert.ToInt64(campo)));
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return alerta;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static AlertaKM cogerAlertaKM(string tipo, string campo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                AlertaKM alerta = (!tipo.Equals("id")) ? ((AlertaKM)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAlertaKmPorDescripcion", campo)) : ((AlertaKM)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAlertaKmPorId", Convert.ToInt64(campo)));
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return alerta;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasAlertasFecha()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasAlertasFecha", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todas las alertas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasAlertasKM()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasAlertasKM", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todas las alertas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static List<AlertaFecha> cogerTodasAlertasFechaEnFecha()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList _alertas = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasAlertasFecha", null);
                List<AlertaFecha> alertas = new List<AlertaFecha>();
                foreach (AlertaFecha item in _alertas)
                {
                    if ((item.fechaLimite - DateTime.Now).Days <= item.diasAntelacion)
                    {
                        alertas.Add(item);
                    }
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return alertas;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar alertas en fecha.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int borrarAlerta(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarAlerta", id);
                MessageBox.Show("Alerta borrada correctamente en la BBDD.", "Alerta borrada", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la eliminación de la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return -1;
            }
        }

        internal static void modificarAlertaFecha(AlertaFecha alerta)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarAlerta", alerta.idAntiguo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarAlertaFecha", alerta);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Alerta modificada correctamente en la BBDD.", "Alerta actualizada", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        internal static void modificarAlertaKM(AlertaKM alerta)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarAlerta", alerta.idAntiguo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarAlertaKM", alerta);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Alerta modificada correctamente en la BBDD.", "Alerta actualizada", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
