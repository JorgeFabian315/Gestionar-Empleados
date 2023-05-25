using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nomina.ViewModels
{
    public class MainViewViewModel : INotifyPropertyChanged
    {
        EmpleadosViewModel empleadosviewmodel = new();
        CategoriasViewModel categoriasviewmodel = new();

        private object actual = null!;

        public object ActualViewModel
        {
            get { return actual; }
            set { actual = value; 
            Actualizar();
            }
        }

        public ICommand NavegarEmpleadosCommand { get; set; }
        public ICommand NavegarCategoriasCommand { get; set; }

        public MainViewViewModel()
        {
            ActualViewModel= empleadosviewmodel;
            NavegarEmpleadosCommand = new RelayCommand(NavegarEmpleados);
            NavegarCategoriasCommand = new RelayCommand(NavegarCategorias);
        }

        private void NavegarCategorias()
        {
            ActualViewModel = categoriasviewmodel;
            categoriasviewmodel.GetCategorias();
            categoriasviewmodel.Recargar(categoriasviewmodel.ListaCategorias);
            Actualizar();
        }

        private void NavegarEmpleados()
        {
            ActualViewModel = empleadosviewmodel;
            Actualizar();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Actualizar(string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
