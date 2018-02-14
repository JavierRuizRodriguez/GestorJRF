using GestorJRF.POJOS;
using GestorJRF.MyBatis.NET;
using System.Windows;
using Npgsql;
using System.Diagnostics;
using System.Collections;

namespace GestorJRF.CRUD
{
    internal class CamionesCRUD
    {
        internal static int añadirCamion(Camion c)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarCamion", c);
                MessageBox.Show("Camion almacenado correctamente en la BBDD.", "Nuevo camión", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El número de bastidor, o la matrícula, introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo camión.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosCamiones", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los camiones.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                MessageBox.Show("Camion modificado correctamente en la BBDD.", "Camión actualizado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error en la actualización del camión.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                MessageBox.Show("Camion borrado correctamente en la BBDD.", "Camión borrado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error en el borrado del camión.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static Camion cogerCamion(string tipo, string campo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Camion camion = (!tipo.Equals("NÚMERO DE MATRÍCULA")) ? ((Camion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerCamionPorBastidor", campo)) : ((Camion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerCamionPorMatricula", campo));
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return camion;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al buscar el camión.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
