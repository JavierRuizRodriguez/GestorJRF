using GestorJRF.POJOS;
using GestorJRF.MyBatis.NET;
using System.Windows;
using Npgsql;
using System.Diagnostics;
using System.Collections;

namespace GestorJRF.CRUD
{
    class CamionesCRUD
    {
        internal static int añadirCamion(Camion c)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarCamion", c);
                MessageBox.Show("Camion almacenado correctamente.", "Nuevo camión", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El número de bastidor, o la matrícula, introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo camión.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static IList cogerTodosCamiones()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosCamiones", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los camiones.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int modificarCamion(Camion camion)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarCamion", camion);
                MessageBox.Show("Camion modificado correctamente.", "Camión actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error en la actualización del camión.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static int borrarCamion(string nBastidor)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarCamion", nBastidor);
                MessageBox.Show("Camion borrado correctamente.", "Camión borrado", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error en el borrado del camión.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static Camion cogerCamion(string tipo, string campo)
        {
            Camion camion;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                if (tipo.Equals("NÚMERO DE MATRÍCULA"))
                    camion = (Camion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerCamionPorMatricula", campo);
                else
                    camion = (Camion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerCamionPorBastidor", campo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return camion;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al buscar el camión.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
