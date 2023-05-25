using Microsoft.EntityFrameworkCore;
using Nomina.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomina.Catalogo
{
   public class CategoriasCatalogo
    {
        NominaContext context = new();
        public IEnumerable<Categorias> GetCategoriasNombre()
        {
            return context.Categorias.OrderBy(x => x.Nombre);
        }
        public void Reload(Categorias categoria)
        {
            context.Entry(categoria).Reload();
        }

        public string GetCategoriaMejorPagada()
        {
            var sueldomaximo = context.Categorias.Max(s => s.SueldoMaximo);
            var f = context.Categorias.Where(d => d.SueldoMaximo == sueldomaximo).FirstOrDefault().Nombre;
            if (f is not null)
                return f;
            else
                return "No hay categorías";
        
        }
        public string GetCategoriaPeorPagada()
        {
            var sueldominimo = context.Categorias.Min(s => s.SueldoMaximo);
           var ca = context.Categorias.Where(d => d.SueldoMaximo == sueldominimo).FirstOrDefault().Nombre;
            if (ca != null)
                return ca;
            else
                return "No hay categorías";
        }




        public string GetCategoriaMasEmpleados()
        {
            var sueldomaximo = context.Categorias.Max(s => s.TotalEmpleados);
            var f = context.Categorias.Where(d => d.TotalEmpleados == sueldomaximo).FirstOrDefault().Nombre;
            if (f is not null)
                return f;
            else
                return "No hay categorías";

        }
        public string GetCategoriaMenosEmpleados()
        {
            var sueldominimo = context.Categorias.Min(s => s.TotalEmpleados);
            var ca = context.Categorias.Where(d => d.TotalEmpleados == sueldominimo).FirstOrDefault().Nombre;
            if (ca != null)
                return ca;
            else
                return "No hay categorías";
        }


        ///////





    


    }
}
