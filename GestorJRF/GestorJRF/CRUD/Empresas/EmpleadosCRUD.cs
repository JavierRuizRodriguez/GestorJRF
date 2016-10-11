using GestorJRF.POJOS;
using Npgsql;
using System.Diagnostics;
using System.Windows;
using GestorJRF.MyBatis.NET;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace GestorJRF.CRUD.Empresas
{
    class EmpleadosCRUD
    {
        internal static int insertarEmpleado(Empleado empleado)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarEmpleado", empleado);
                MessageBox.Show("El empleado ha sido creado correctamente.", "Empleado creado", MessageBoxButton.OK, MessageBoxImage.Information);
                return 1;
            }
            catch (NpgsqlException ex)
            {
                Trace.WriteLine(ex.ToString());
                if (ex.ErrorCode == -2147467259)
                {
                    MessageBox.Show("El dni introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Error en la creación del nuevo empleado.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return -1;
            }
        }

        internal static Empleado cogerEmpleado(string tipo, string campo)
        {
            try
            {
                if(tipo.Equals("DNI"))
                    return (Empleado)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpleadoPorDni", campo);
                else
                {                    
                    List<string> parametros = campo.Split(',').ToList();
                    Nombre nombre = new Nombre(parametros[1], parametros[0]);
                    nombre.nombre = Regex.Replace(nombre.nombre, @"\s+", "");
                    return (Empleado)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpleadoPorNombreApellidos", nombre);
                }
            }
            catch (NpgsqlException ex)
            {
                Trace.WriteLine(ex.ToString());                
                return null;
            }
        }

        internal static int modificarEmpleado(Empleado empleado)
        {
            int salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarEmpleado", empleado);
                MessageBox.Show("Empleado modificado correctamente.", "Empleado actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NpgsqlException ex)
            {
                salida = -1;
                MessageBox.Show("Error en la actualización del empleado.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
            }

            return salida;
        }

        internal static int borrarEmpleado(string dni)
        {
            int salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarEmpleado", dni);
                MessageBox.Show("Empleado borrado correctamente.", "Empleado borrado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Error en el borrado del empleado.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
            }

            return salida;
        }
    }
}
