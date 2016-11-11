using GestorJRF.MyBatis.NET;
using GestorJRF.POJOS;
using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System;

namespace GestorJRF.CRUD
{
    class EmpresasCRUD
    {
        internal static int insertarEmpresa(Empresa empresa)
        {
            var salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarEmpresa", empresa);
                añadirPersonasContacto(empresa);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                if (ex.SqlState.Equals("23505"))
                    MessageBox.Show("El CIF/NIF introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error en la creación de la nueva empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);

                salida = -1;
            }
            return salida;
        }

        private static void añadirPersonasContacto(Empresa empresa)
        {
            try
            {
                foreach (PersonaContacto p in empresa.personasContacto)
                {
                    p.cif = empresa.cif;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarPersonaContacto", p);
                }
            }
            catch (PostgresException ex)
            {
                Trace.WriteLine(ex.ToString());
                throw ex;
            }
        }

        internal static Empresa cogerEmpresa(string tipo, string campo)
        {
            Empresa empresa;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                if (tipo == "nombre")
                    empresa = (Empresa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpresaPorNombre", campo);
                else
                    empresa = (Empresa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpresaPorCif", campo);

                if (empresa != null)
                    empresa.personasContacto = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerPersonasContacto", empresa.cif).Cast<PersonaContacto>().ToList();
                else
                    MessageBox.Show("La empresa buscada no existe.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);

                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return empresa;

            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("Error al buscar la empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static IList cogerTodasEmpresas()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasEmpresas", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todas las empresas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodoPersonalContacto(string cif)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                var lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerPersonasContacto", cif);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todas las personas de contacto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static void modificarPersonasContacto(Empresa empresa, string cifAntiguo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarPersonasContacto", cifAntiguo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarEmpresa", empresa);
                añadirPersonasContacto(empresa);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Empresa modificada correctamente.", "Empresa actualizada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        internal static int borrarEmpresa(string cif)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarEmpresa", cif);
                MessageBox.Show("Empresa borrada correctamente.", "Empresa borrada", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
