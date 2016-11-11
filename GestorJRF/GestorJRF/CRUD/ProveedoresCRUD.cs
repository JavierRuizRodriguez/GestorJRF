using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using Npgsql;
using System.Collections;
using System.Diagnostics;
using System.Windows;

namespace GestorJRF.CRUD
{
    class ProveedoresCRUD
    {
        internal static int añadirProveedor(Proveedor proveedor)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarProveedor", proveedor);
                MessageBox.Show("Proveedor almacenado correctamente.", "Nuevo proveedor", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El proveedor introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo proveedor.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Trace.WriteLine(ex.ToString());
                return -1;
            }
        }

        internal static Proveedor cogerProveedor(string tipo, string campo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Proveedor proveedor;
                if (tipo.Equals("cif"))
                    proveedor = (Proveedor)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerProveedorPorCif", campo);
                else
                    proveedor = (Proveedor)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerProveedorPorNombre", campo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return proveedor;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger el proveedor.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosProveedores()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosProveedores", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todos los proveedores.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int borrarProveedor(string cif)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarProveedor", cif);
                MessageBox.Show("Proveedor borrado correctamente.", "Proveedor borrado", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la eliminación del proveedor.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        internal static void modificarProveedor(Proveedor proveedor)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarProveedor", proveedor);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Proveedor modificado correctamente.", "Proveedor modificado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación del provedor.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
