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
    internal class EmpresasCRUD
    {
        internal static int insertarEmpresa(Empresa empresa)
        {
            int salida = 1;
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarEmpresa", empresa);
                EmpresasCRUD.añadirPersonasContacto(empresa);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Empresa almacenada correctamente en la BBDD.", "Nueva empresa", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                if (ex.SqlState.Equals("23505"))
                {
                    MessageBox.Show("El CIF/NIF introducido ya está almacenado.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error en la creación de la nueva empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                salida = -1;
            }
            return salida;
        }

        private static void añadirPersonasContacto(Empresa empresa)
        {
            try
            {
                foreach (PersonaContacto item in empresa.personasContacto)
                {
                    item.cif = empresa.cif;
                    InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Insert("insertarPersonaContacto", item);
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
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                Empresa empresa;
                if (tipo == "nombre")
                {
                    string campoBusqueda = "%" + campo + "%";
                    empresa = (Empresa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpresaPorNombre", campoBusqueda);
                }
                else
                {
                    empresa = (Empresa)InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForObject("cogerEmpresaPorCif", campo);
                }
                if (empresa != null)
                {
                    empresa.personasContacto = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerPersonasContacto", empresa.cif).Cast<PersonaContacto>().ToList();
                }
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return empresa;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("Error al buscar la empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return null;
            }
        }

        internal static IList cogerTodasEmpresas()
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerTodasEmpresas", null);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todas las empresas.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static IList cogerTodoPersonalContacto(string cif)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                IList lista = InstanciaPostgreSQL.CogerInstaciaPostgreSQL.QueryForList("cogerPersonasContacto", cif);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return lista;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                MessageBox.Show("Error al buscar todas las personas de contacto.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }

        internal static int modificarPersonasContacto(Empresa empresa, string cifAntiguo)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarPersonasContacto", cifAntiguo);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Update("actualizarEmpresa", empresa);
                EmpresasCRUD.añadirPersonasContacto(empresa);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                MessageBox.Show("Empresa modificada correctamente en la BBDD.", "Empresa actualizada", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la modificación de la empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return -1;
            }
        }

        internal static int borrarEmpresa(string cif)
        {
            try
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.BeginTransaction();
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.Delete("borrarEmpresa", cif);
                MessageBox.Show("Empresa borrada correctamente en la BBDD.", "Empresa borrada", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.CommitTransaction();
                return 1;
            }
            catch (PostgresException ex)
            {
                InstanciaPostgreSQL.CogerInstaciaPostgreSQL.RollBackTransaction();
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("No ha sido posible la eliminación de la empresa.", "Aviso error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return -1;
            }
        }
    }
}
