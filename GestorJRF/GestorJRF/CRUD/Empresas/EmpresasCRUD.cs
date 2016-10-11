using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System;

namespace GestorJRF.CRUD.Empresas
{
    class EmpresasCRUD
    {
        internal static int insertarEmpresa(Empresa empresa)
        {
            var salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarEmpresa", empresa);
                foreach (PersonaContacto p in empresa.personasContacto)
                {
                    p.cif = empresa.cif;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarPersonaContacto", p);
                }
            }
            catch (NpgsqlException ex)
            {
                Trace.WriteLine(ex.ToString());
                if (ex.ErrorCode == -2147467259)
                {
                    salida = -1;
                }
                else
                {
                    salida = -2;
                }
            }
            return salida;
        }

        public static Empresa cogerEmpresa(string tipo, string campo)
        {
            Empresa empresa;
            try
            {
                if (tipo == "nombre")
                    empresa = (Empresa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpresaPorNombre", campo);
                else
                    empresa = (Empresa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpresaPorCif", campo);

                if (empresa != null)
                    empresa.personasContacto = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerPersonasContacto", empresa.cif).Cast<PersonaContacto>().ToList();

            }
            catch (NpgsqlException ex)
            {
                Trace.WriteLine(ex.ToString());
                empresa = null;
            }
            return empresa;
        }

        internal static IList cogerTodasEmpresas()
        {
            try
            {
                return InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasEmpresas", null);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Error al buscar todas las empresas.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static void modificarPersonasContacto(Empresa empresa, string cifAntiguo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarPersonasContacto", cifAntiguo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarEmpresa", empresa);
                foreach (PersonaContacto p in empresa.personasContacto)
                {
                    p.cif = empresa.cif;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarPersonaContacto", p);
                }

                MessageBox.Show("Empresa modificada correctamente.", "Empresa actualizada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NpgsqlException ex)
            {
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la empresa.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        internal static int borrarEmpresa(string cif)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarEmpresa", cif);
                MessageBox.Show("Empresa borrada correctamente.", "Empresa borrada", MessageBoxButton.OK, MessageBoxImage.Information);
                return 1;
            }
            catch (NpgsqlException ex)
            {
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la empresa.", "Aviso error fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }
    }
}
