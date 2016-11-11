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
    class EmpleadosCRUD
    {
        internal static int insertarEmpleado(Empleado empleado)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarEmpleado", empleado);
                MessageBox.Show("El empleado ha sido creado correctamente.", "Empleado creado", MessageBoxButton.OK, MessageBoxImage.Information);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El dni introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo empleado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return -1;
            }
        }

        internal static Empleado cogerEmpleado(string tipo, string campo)
        {
            try
            {
                Empleado empleado;
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                if (tipo.Equals("DNI"))
                    empleado = (Empleado)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpleadoPorDni", campo);
                else
                {
                    List<string> parametros = campo.Split(',').ToList();
                    DobleCadena nombre = new DobleCadena(parametros[1], parametros[0]);
                    nombre.cadena1 = Regex.Replace(nombre.cadena1, @"\s+", "");
                    empleado = (Empleado)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpleadoPorNombreApellidos", nombre);
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return empleado;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger el empleado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodosEmpleados", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                MessageBox.Show("Error al coger todos los empleados.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Empleado modificado correctamente.", "Empleado actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                salida = -1;
                MessageBox.Show("Error en la actualización del empleado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Empleado borrado correctamente.", "Empleado borrado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error en el borrado del empleado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
            }

            return salida;
        }
    }
}
