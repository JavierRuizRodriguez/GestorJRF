using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using Npgsql;
using System.Diagnostics;
using System.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using IBatisNet.DataMapper.Exceptions;

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
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarAlertaFecha", alerta);
                else
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarAlertaKM", alerta);
                MessageBox.Show("Alerta almacenada correctamente.", "Nueva alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("La alerta introducida ya está almacenada.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación de la nueva alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static AlertaFecha cogerAlertaFecha(string tipo, string campo)
        {
            AlertaFecha alerta;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                if (tipo.Equals("id"))
                    alerta = (AlertaFecha)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAlertaFechaPorId", Convert.ToInt64(campo));
                else
                    alerta = (AlertaFecha)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAlertaFechaPorDescripcion", campo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return alerta;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static AlertaKM cogerAlertaKM(string tipo, string campo)
        {
            AlertaKM alerta;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                if (tipo.Equals("id"))
                    alerta = (AlertaKM)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAlertaKmPorId", Convert.ToInt64(campo));
                else
                    alerta = (AlertaKM)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAlertaKmPorDescripcion", campo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return alerta;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasAlertasFecha()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasAlertasFecha", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todas las alertas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasAlertasKM()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasAlertasKM", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todas las alertas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                foreach (AlertaFecha a in _alertas)
                {
                    if ((a.fechaLimite - DateTime.Now).Days <= a.diasAntelacion)
                        alertas.Add(a);
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return alertas;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar alertas en fecha.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasAlertasKMenKMs()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList _alertasKM = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasAlertasKM", null);
                List<AlertaKM> alertasKM = new List<AlertaKM>();
                foreach (AlertaKM a in _alertasKM)
                {
                    long km = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerKilometrosCamion", a.matricula);
                    if (a.kmLimite - km <= a.kmAntelacion)
                        alertasKM.Add(a);
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return alertasKM;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar alertas en fecha.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Alerta borrada correctamente.", "Alerta borrada", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la eliminación de la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Alerta modificada correctamente.", "Alerta actualizada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Alerta modificada correctamente.", "Alerta actualizada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la alerta.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
