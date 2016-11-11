using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using Npgsql;
using System;
using System.Diagnostics;
using System.Windows;

namespace GestorJRF.CRUD
{
    class UsuarioSistemaCRUD
    {
        internal static int añadirUsuarioSistema(UsuarioSistema usuario)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarUsuarioSistema", usuario);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Usuario del sistema creado correctamente.", "Nueva usuario sistema", MessageBoxButton.OK, MessageBoxImage.Information);
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El usuario escogido ya está utilizado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo usuario.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static object cogerUsuarioSistema(UsuarioSistema usuario)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var salida = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerUsuarioSistema", usuario);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return salida;
            }
            catch (Exception ex)
            {
                if (ex is PostgresException)
                {
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                    MessageBox.Show("Error al buscar la el usuario del sistema.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("No es posible conectase con el servidor. Vuelva a intentarlo cuando el servidor esté disponible.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                }
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static string modificacionAutomaticaContraseña(UsuarioSistema usuario)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                string email = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("actualizarUsuarioSistemaContraseña", usuario) as string;
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return email;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la generación de la nueva contraseña.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static int modificacionUsuarioSistema(UsuarioSistema usuario)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarUsuarioSistema", usuario);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación del usuario.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }
    }
}
