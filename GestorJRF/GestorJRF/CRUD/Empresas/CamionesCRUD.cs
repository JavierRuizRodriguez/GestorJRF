using GestorJRF.POJOS;
using GestorJRF.MyBatis.NET;
using System.Windows;
using Npgsql;
using System;
using System.Diagnostics;
using System.Collections;

namespace GestorJRF.CRUD.Empresas
{
    class CamionesCRUD
    {
        internal static int añadirCamion(Camion c)
        {
            int salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarCamion", c);
                MessageBox.Show("Camion almacenado correctamente.", "Nuevo camión", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NpgsqlException ex)
            {
                salida = -1;
                if (ex.ErrorCode == -2147467259)
                {
                    MessageBox.Show("El número de bastidor, o la matrícula, introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo camión.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
            }
            return salida;
        }

        internal static IList cogerTodosCamiones()
        {
            try
            {
                return InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosCamiones", null);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Error al buscar todos los camiones.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int modificarCamion(Camion camion)
        {
            int salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarCamion", camion);
                MessageBox.Show("Camion modificado correctamente.", "Camión actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NpgsqlException ex)
            {
                salida = -1;
                MessageBox.Show("Error en la actualización del camión.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
            }

            return salida;
        }

        internal static int borrarCamion(string nBastidor)
        {
            int salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarCamion", nBastidor);
                MessageBox.Show("Camion borrado correctamente.", "Camión borrado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Error en el borrado del camión.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
            }

            return salida;
        }

        internal static Camion cogerCamion(string tipo, string campo)
        {
            try
            {
                if (tipo.Equals("NÚMERO DE MATRÍCULA"))
                    return (Camion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerCamionPorMatricula", campo);
                else
                    return (Camion)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerCamionPorBastidor", campo);
            }
            catch (NpgsqlException ex)
            {
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
