using Nomina.Catalogo;
using Nomina.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomina.ViewModels
{
    public class CategoriasViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Categorias> ListaCategorias{ get; set; } = new();
        public ObservableCollection<Empleados> ListaEmpleados { get; set; } = new();

        public CategoriasCatalogo ccategorias = new();
        EmpleadosCatalogo cempleados = new();

        public string CategoriaMejorPagada { get; set; }
        public string CategoriaPeorPagada { get; set; }
        public string CategoriaMasEmpleados { get; set; }
        public string CategoriaMenosEmpleados { get; set; }
        public Categorias Categoria { get; set; }

        public CategoriasViewModel()
        {
            GetCategorias();
            Estadisticas();
            Actualizar();
        }
        public void Estadisticas()
        {
            CategoriaMejorPagada = ccategorias.GetCategoriaMejorPagada();
            CategoriaPeorPagada = ccategorias.GetCategoriaPeorPagada();
            CategoriaMasEmpleados = ccategorias.GetCategoriaMasEmpleados();
            CategoriaMenosEmpleados = ccategorias.GetCategoriaMenosEmpleados();

        }
        public void GetCategorias()
        {
            ListaCategorias.Clear();
            //ccategorias.Reload(Categoria);
            foreach (var item in ccategorias.GetCategoriasNombre())
            {
                ListaCategorias.Add(item);
                //ccategorias.Reload(item);
            }
            Actualizar();
        }
        public void GetEmpleados()
        {
            ListaEmpleados.Clear();
            foreach (var item in cempleados.GetEmpleados())
            {
                ListaEmpleados.Add(item);
            }
            Actualizar();

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void Actualizar(string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Recargar(ObservableCollection<Categorias> lista)
        {
            foreach (var categoria in lista)
            {
                ccategorias.Reload(categoria);
            }
        }

    }
}
