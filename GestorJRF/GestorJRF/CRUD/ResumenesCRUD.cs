﻿using GestorJRF.POJOS;
using GestorJRF.MyBatis.NET;
using System.Windows;
using Npgsql;
using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GestorJRF.POJOS.Mapas;
using GestorJRF.POJOS.Facturas;
using GestorJRF.POJOS.Estadisticas;

namespace GestorJRF.CRUD
{
    class ResumenesCRUD
    {
        internal static int añadirResumenPrevio(Resumen resumen)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                long id = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarResumenPrevio", resumen);
                añadirItinerariosResumenPrevio(resumen, id);
                MessageBox.Show("Resumen almacenado correctamente.", "Nuevo resumen", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El resumen ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static int añadirResumenFinal(Resumen resumen)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarResumenPrevio", resumen.id);
                long id = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarResumenFinal", resumen);
                añadirItinerariosComisionesResumenFinal(resumen, id);
                MessageBox.Show("Resumen almacenado correctamente.", "Nuevo resumen", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El resumen ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        private static void añadirItinerariosResumenPrevio(Resumen resumen, long id)
        {
            try
            {
                foreach (Itinerario itinerario in resumen.listaItinerarios)
                {
                    itinerario.idResumen = id;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarItinerarioResumenPrevio", itinerario);
                }
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("Algún servicio del resumen ya está almacenado", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación de los servicios del resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                throw ex;
            }
        }

        private static void añadirItinerariosComisionesResumenFinal(Resumen resumen, long id)
        {
            try
            {
                foreach (Itinerario servicio in resumen.listaItinerarios)
                {
                    servicio.idResumen = id;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarItinerarioResumenFinal", servicio);
                }
                foreach (Comision comision in resumen.listaComisiones)
                {
                    comision.idResumenFinal = id;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarComisionResumen", comision);
                }
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("Algún servicio del resumen ya está almacenado", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación de los servicios del resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                throw ex;
            }

        }

        internal static Resumen cogerResumenPrevio(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Resumen resumen = (Resumen)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerResumenPrevioPorId", id);
                if (resumen != null)
                    resumen.listaItinerarios = new List<Itinerario>(InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosItinerariosPorIdResumenPrevio", resumen.id).Cast<Itinerario>().ToList());
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return resumen;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger el resumen previo.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static Resumen cogerResumenFinal(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Resumen resumen = (Resumen)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerResumenFinalPorId", id);
                if (resumen != null)
                {
                    resumen.listaItinerarios = new List<Itinerario>(InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosItinerariosPorIdResumenFinal", resumen.id).Cast<Itinerario>().ToList());
                    resumen.listaComisiones = new List<Comision>(InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerComisionResumenPorId", resumen.id).Cast<Comision>().ToList());
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return resumen;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger el resumen final.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static double cogerResumenesFinalesPorCif(BusquedaEstadisticas busqueda)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var sumatorio = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerResumenesFinalesPorCif", busqueda);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                if (sumatorio != null)
                    return Convert.ToDouble(sumatorio);
                else
                    return 0;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger el resumen final.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return 0;
            }
        }

        internal static double cogerResumenesFinalesPorDni(BusquedaEstadisticas busqueda)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var sumatorio = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerResumenesFinalesPorDni", busqueda);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                if (sumatorio != null)
                    return Convert.ToDouble(sumatorio);
                else
                    return 0;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger el resumen final.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return 0;
            }
        }

        internal static IList cogerTodosResumenesPrevios()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosResumenesPrevios", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todos los resumenes previos.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosResumenesFinales()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosResumenesFinales", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todos los resumenes finales.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosItinerariosPorIdResumenPrevio(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosItinerariosPorIdResumenPrevio", id);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todos los itinerarios previos por resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosItinerariosPorIdResumenFinal(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosItinerariosPorIdResumenFinal", id);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todos los itinerarios finales por resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodasComisionesPorIdResumenFinal(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerComisionResumenPorId", id);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todos los itinerarios finales por resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerResumenesParaFactura(BusquedaFactura busqueda)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList resumenes;
                if (!busqueda.referencia.Equals(""))
                    resumenes = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerResumenesParaFacturaReferencia", busqueda);
                else
                    resumenes = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerResumenesParaFactura", busqueda);

                if (resumenes != null)
                {
                    foreach (Resumen r in resumenes)
                    {
                        r.listaItinerarios = new List<Itinerario>(InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosItinerariosPorIdResumenFinal", r.id).Cast<Itinerario>().ToList());
                    }
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return resumenes;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger resumenes para factura.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerResumenesParaEstadistica(BusquedaEstadisticas busqueda)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList resumenes = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerResumenesParaEstadisticas", busqueda);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return resumenes;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger resumenes para estadisticas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerResumenesParaEstadisticaPorEmpresa(BusquedaEstadisticas busqueda)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList resumenes = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerResumenesParaEstadisticasPorEmpresa", busqueda);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return resumenes;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger resumenes para estadisticas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerResumenesParaEstadisticaPorEmpleado(BusquedaEstadisticas busqueda)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList resumenes = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerResumenesParaEstadisticasPorEmpleado", busqueda);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return resumenes;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger resumenes para estadisticas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int modificarResumenPrevio(Resumen resumen)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarResumenPrevio", resumen.idAntiguo);
                long id = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarResumenPrevio", resumen);
                añadirItinerariosResumenPrevio(resumen, id);
                MessageBox.Show("Empresa modificada correctamente.", "Empresa actualizada", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        internal static int modificarResumenFinal(Resumen resumen)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarResumenFinal", resumen.idAntiguo);
                long id = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarResumenFinal", resumen);
                añadirItinerariosComisionesResumenFinal(resumen, id);
                MessageBox.Show("Empresa modificada correctamente.", "Empresa actualizada", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        internal static int borrarResumenPrevio(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarResumenPrevio", id);
                MessageBox.Show("Resumen borrado correctamente.", "Resumen borrado", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación del resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        internal static int borrarResumenFinal(long id)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarResumenFinal", id);
                MessageBox.Show("Resumen borrado correctamente.", "Resumen borrado", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación del resumen.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }


        internal static long cogerNumeroFactura()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var salida = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerSiguienteNumeroFactura", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return salida;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return 0;
            }
        }

        internal static long CancelarNumeroFactura()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var salida = (long)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerAnteriorNumeroFactura", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return salida;

            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return 0;
            }
        }
    }
}
