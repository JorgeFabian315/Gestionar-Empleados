using Microsoft.EntityFrameworkCore;
using Nomina.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nomina.Catalogo
{
    public class EmpleadosCatalogo
    {
        NominaContext context = new();
        public IEnumerable<Empleados> GetEmpleados()
        {
            return context.Empleados.Include(x => x.Categoria).Where(c => c.Activo == true).OrderBy(d => d.Nombre);
        }
        public IEnumerable<Empleados> GetEmpleadosEliminados()
        {
            return context.Empleados.Include(x => x.Categoria).Where(c => c.Activo == false).OrderBy(d => d.Nombre);
        }
        public Empleados BuscarxId(int id)
        {
            var empleado = context.Empleados.Find(id);
            if (empleado != null)
                return empleado;
            else
                return null;
        }



        public void Create(Empleados em)
        {
            context.Add(em);
            //var cate = context.Categorias.Where(c => c.Id == em.CategoriaId).SingleOrDefault();
            //if(cate != null)
            //    Reload(cate);

            context.SaveChanges();
        }

        public void Reload(Empleados em)
        {
            context.Entry(em).Reload();
        }
        public void Reload(int id)
        {
            var ca = context.Categorias.Find(id);
            if (ca is not null)
                context.Entry(ca).Reload();
        }
        public void Delete(int id)
        {
            //context.Empleados.Where(x => x.Id == id).ExecuteDelete();

            var empleadoeliminado = context.Empleados.Find(id);

            if (empleadoeliminado is not null)
            {
                context.Categorias.Where(c => c.Id == empleadoeliminado.CategoriaId).ExecuteUpdate(s => s.SetProperty(j => j.TotalEmpleados, o => o.TotalEmpleados - 1));
                context.Empleados.Where(x => x.Id == id).ExecuteUpdate(p => p.SetProperty(j => j.Activo, false));
            }


        }
        public void Activar(int id)
        {
            var empleadoeliminado = context.Empleados.Find(id);

            if (empleadoeliminado is not null)
            {
                context.Categorias.Where(c => c.Id == empleadoeliminado.CategoriaId).ExecuteUpdate(s => s.SetProperty(j => j.TotalEmpleados, o => o.TotalEmpleados + 1));
                context.Empleados.Where(x => x.Id == id).ExecuteUpdate(p => p.SetProperty(j => j.Activo, true));

            }
        }
        public void Update(Empleados em)
        {

            context.Entry(em.Categoria).State = EntityState.Unchanged;
            context.Update(em);
            context.SaveChanges();
        }


        public bool Validar(Empleados empleado, out List<string> lista)
        {
            lista = new();
            string cadenanombre = @"^[A-Za-z]{3,}(\s[A-Za-z]{3,}){0,3}$";
            Regex validarnombre = new Regex(cadenanombre);

            if (empleado is not null)
            {
                if (string.IsNullOrWhiteSpace(empleado.Nombre))
                    lista.Add("El nombre del empleado no puede estar vacío");
                else if (empleado.Nombre.Length > 100)
                    lista.Add("El nombre del empleado ha superado el tamaño establecido");
                else if(!validarnombre.IsMatch(empleado.Nombre))
                    lista.Add("El nombre del empleado tiene el formato incorrecto");

                else if (context.Empleados.Any(x => x.Nombre == empleado.Nombre && x.Id != empleado.Id))
                    lista.Add("El empleado ya existe");

                if (empleado.Sueldo is null)
                    lista.Add("El sueldo es incorrecto");
                else if (empleado.Sueldo <= 0)
                    lista.Add("El sueldo es incorrecto");

                if (empleado.CategoriaId <= 0)
                    lista.Add("La categoría es incorrecta");

                return lista.Count == 0;
            }
            return false;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        public IEnumerable<Empleados> GetEmpleadosUltimo()
        {
            return context.Empleados.Include(x => x.Categoria).Where(c => c.Activo == true).OrderByDescending(c => c.Nombre);
        }
        public IEnumerable<Empleados> GetEmpleadosSueldo()
        {
            return context.Empleados.Include(x => x.Categoria).Where(c => c.Activo == true).OrderByDescending(c => c.Sueldo);
        }

        public string GetEmpleadoMejorPagado()
        {
            var sueldomaximo = context.Empleados.Max(c => c.Sueldo);
            var t = context.Empleados.Where(s => s.Sueldo == sueldomaximo).FirstOrDefault().Nombre;
            return t ?? "No hay empleados";
        }
        public string GetEmpleadoPeorPagado()
        {
            var sueldominimo = context.Empleados.Min(c => c.Sueldo);
            var t = context.Empleados.Where(s => s.Sueldo == sueldominimo).FirstOrDefault().Nombre;
            return t ?? "No hay empleados";

        }

        public string GetSueldoMaximo()
        {
          return context.Empleados.Max(c => c.Sueldo).Value.ToString() ?? "0.00";
        }
        public string GetMinimoMaximo()
        {
            return context.Empleados.Min(c => c.Sueldo).Value.ToString() ?? "0.00";
        }







    }
}
