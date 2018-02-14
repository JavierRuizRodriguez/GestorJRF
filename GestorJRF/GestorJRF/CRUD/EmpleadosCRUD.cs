using GestorJRF.POJOS;
using Npgsql;
using System.Diagnostics;
using System.Windows;
using GestorJRF.MyBatis.NET;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;
namespace GestorJRF.CRUD
{
    internal class EmpleadosCRUD
    {
        internal static int insertarEmpleado(Empleado empleado)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarEmpleado", empleado);
                MessageBox.Show("El empleado ha sido creado correctamente en la BBDD.", "Empleado creado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El dni introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo empleado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                return -1;
            }
        }

        internal static Empleado cogerEmpleado(string tipo, string campo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Empleado empleado;
                if (tipo.Equals("DNI"))
                {
                    empleado = (Empleado)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpleadoPorDni", campo);
                }
                else
                {
                    string campoBusqueda = "%" + campo + "%";
                    empleado = (Empleado)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpleadoPorNombreApellidos", campoBusqueda);
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return empleado;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger el empleado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodosEmpleados()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosEmpleados", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todos los empleados.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int modificarEmpleado(Empleado empleado)
        {
            int salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarEmpleado", empleado);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Empleado modificado correctamente en la BBDD.", "Empleado actualizado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                salida = -1;
                MessageBox.Show("Error en la actualización del empleado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
            }
            return salida;
        }

        internal static int borrarEmpleado(string dni)
        {
            int salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarEmpleado", dni);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Empleado borrado correctamente en la BBDD.", "Empleado borrado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error en el borrado del empleado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
            }
            return salida;
        }
    }
}
