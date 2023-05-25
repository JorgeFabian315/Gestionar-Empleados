using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight.Command;
using Nomina.Catalogo;
using Nomina.Models;
using Nomina.Views;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nomina.ViewModels
{
    public class EmpleadosViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Empleados> ListaEmpleados { get; set; } = new ObservableCollection<Empleados>();
        public ObservableCollection<Empleados> ListaEmpleadosEliminados { get; set; } = new ObservableCollection<Empleados>();
        public ObservableCollection<Categorias> ListaCategorias { get; set; } = new ObservableCollection<Categorias>();

        EmpleadosCatalogo cempleados = new EmpleadosCatalogo();
        CategoriasCatalogo ccategorias = new();

        public ICommand VerEliminarEmpleadoCommand { get; set; }
        public ICommand EliminarEmpleadoCommand { get; set; }
        public ICommand VerAgregarEmpleadoCommand => new RelayCommand(VerAgregarEmpleado);
        public ICommand AgregarEmpleadoCommand => new RelayCommand<int>(AgregarEmpleado);
        public ICommand VerEmpleadosEliminadosCommand { get; set; }
        public ICommand VerEditarEmpleadoCommand { get; set; }
        public ICommand EditarEmpleadoCommand { get; set; }
        public ICommand VerEstadisticasCommand => new RelayCommand(VerEstadisticas);
        public ICommand ActivarEmpleadoCommand { get; set; }
        public ICommand ExportarExcelCommand { get; set; }
        public ICommand OrdenarEmpleadosCommand { get; set; }
        public ICommand RegresarCommand { get; set; }
        
        public Empleados Empleado { get; set; } = new();

        public int TotalEmpleados { get; set; }
        public int TotalEmpleadosEliminados { get; set; }
        public Vista Vista { get; set; }
        public string Error { get; set; } = "";
        public string RutaExcel { get; set; } = "";


        public string EmpleadoMejorPagado { get; set; }
        public string EmpleadoPeorPagado { get; set; }
        public string PeorSueldo { get; set; }
        public string MejorSueldo { get; set; }






        public EmpleadosViewModel()
        {
            GetEmpleados();
            // VerAgregarEmpleadoCommand = new RelayCommand(VerAgregarEmpleado);
           // AgregarEmpleadoCommand = new RelayCommand<int>(AgregarEmpleado);
            VerEliminarEmpleadoCommand = new RelayCommand<int>(VerEliminarEmpleado);
            EliminarEmpleadoCommand = new RelayCommand(EliminarEmpleado);
            RegresarCommand = new RelayCommand(Regresar);
            VerEmpleadosEliminadosCommand = new RelayCommand(VerEmpleadosEliminados);
            VerEditarEmpleadoCommand = new RelayCommand<int>(VerEditarEmpleado);
            EditarEmpleadoCommand = new RelayCommand<int>(EditarEmpleado);
            ActivarEmpleadoCommand = new RelayCommand<int>(ActivarEmpleado);
            ExportarExcelCommand = new RelayCommand(ExportarExcel);
            OrdenarEmpleadosCommand = new RelayCommand<string>(OrdenarEmpleados);
            GetCategorias();

            TotalEmpleados = ListaEmpleados.Count();

            Actualizar();
        }
        public void VerEstadisticas()
        {
            Vista = Vista.Estadisticas;
            Estadisticas();
            Actualizar();
        }
        public void Estadisticas()
        {
            EmpleadoMejorPagado = cempleados.GetEmpleadoMejorPagado();
            EmpleadoPeorPagado = cempleados.GetEmpleadoPeorPagado();
            MejorSueldo = cempleados.GetSueldoMaximo();
            PeorSueldo = cempleados.GetMinimoMaximo();
            Actualizar();
        }

        private void OrdenarEmpleados(string parametro)
        {
            ListaEmpleados.Clear();
            if(parametro == "a-z")
                foreach (var item in cempleados.GetEmpleados())
                    ListaEmpleados.Add(item);
            else if (parametro == "z-a")
                foreach (var item in cempleados.GetEmpleadosUltimo())
                    ListaEmpleados.Add(item);
            else if (parametro == "sueldo")
                foreach (var item in cempleados.GetEmpleadosSueldo())
                    ListaEmpleados.Add(item);
            
            Actualizar();
        }

        public void GetCategorias()
        {
            ListaCategorias.Clear();
            foreach (var item in ccategorias.GetCategoriasNombre())
            {
                ListaCategorias.Add(item);
            }
        }


        private void ExportarExcel()
        {
            // string rutaexce = @"C:\Users\Fabian\Music\Materias\Administracion base de datos\Nomina\Nomina\bin\Debug\net6.0-windows\Execel\Empleados.xlsx";
            string rutaexcel = $@"{AppDomain.CurrentDomain.BaseDirectory}\Excel\Empleados.xlsx";
            SLDocument doc = new();

            #region ESTILOS EXCEL
            SLStyle titulos = new();
            SLStyle contenido = new();
            titulos.Font.Bold = true;
            titulos.Font.FontSize = 20;
            titulos.Alignment.Horizontal = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center;
            titulos.Alignment.Vertical = DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center;
            titulos.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Blue,System.Drawing.Color.Blue);
            titulos.Font.FontColor = System.Drawing.Color.White;
            titulos.Border.LeftBorder.BorderStyle = BorderStyleValues.Thick;
            titulos.Border.BottomBorder.BorderStyle = BorderStyleValues.Thick;
            titulos.Border.RightBorder.BorderStyle = BorderStyleValues.Thick;
            titulos.Border.TopBorder.BorderStyle = BorderStyleValues.Thick;

            contenido.Font.FontSize = 15;
            contenido.Alignment.Horizontal = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center;
            contenido.Alignment.Vertical = DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center;
            contenido.Border.LeftBorder.BorderStyle = BorderStyleValues.Thick;
            contenido.Border.BottomBorder.BorderStyle = BorderStyleValues.Thick;
            contenido.Border.RightBorder.BorderStyle = BorderStyleValues.Thick;
            contenido.Border.TopBorder.BorderStyle = BorderStyleValues.Thick;
            #endregion  

           
            // DEFINIR COLUMNAS
            doc.SetCellValue(1,1,"Id");
            doc.SetCellValue(1, 2, "Nombre");
            doc.SetCellValue(1, 3, "Sueldo");
            doc.SetCellValue(1, 4, "Categoría");
            // END DEFINIR COLUMNAS

            // ESTABLECER LOS ESTILOS, FILA, COLUMNA
            doc.SetCellStyle(1, 1, titulos);
            doc.SetCellStyle(1, 2, titulos);
            doc.SetCellStyle(1, 3, titulos);
            doc.SetCellStyle(1, 4, titulos);
            // END ESTABLECER LOS ESTILOS, FILA, COLUMNA

            // DEFINIR EL ANCHO DE LAS COLUMNAS
            doc.SetColumnWidth(1,1,20);
            doc.SetColumnWidth(1, 2, 25);
            doc.SetColumnWidth(1, 3, 25);
            doc.SetColumnWidth(1, 4, 25);
            // END DEFINIR EL ANCHO DE LAS COLUMNAS

            // DEFINIR EL CONTENIDO DEL EXCEL 
            int i = 2;
            var d = cempleados.GetEmpleados().OrderBy(x => x.Id);
            foreach (var empleado in d)
            {
                doc.SetCellValue(i,1,empleado.Id);// NUMERO DE FILA, NUMERO DE COLUMNA, CONTENIDO 
                doc.SetCellValue(i,2,empleado.Nombre);
                doc.SetCellValue(i,3,$"$ {empleado.Sueldo}");
                doc.SetCellValue(i,4,empleado.Categoria.Nombre);
                // ESTILOS 
                doc.SetCellStyle(i,1,contenido);
                doc.SetCellStyle(i,2,contenido);
                doc.SetCellStyle(i,3,contenido);
                doc.SetCellStyle(i,4,contenido);
                i++;
            }
            // END DEFINIR EL CONTENIDO DEL EXCEL 

            // GUARDAR Y CREAR EL ARCHIVO EN LA RUTA ESTABLECIDA
            doc.SaveAs(rutaexcel);
            // END GUARDAR Y CREAR EL ARCHIVO EN LA RUTA ESTABLECIDA

            // ABRIR EL ARCHIVO EXCEL
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(rutaexcel)
            {
                UseShellExecute = true
            };
            p.Start();
            // END ABRIR EL ARCHIVO EXCEL
        }

        private void ActivarEmpleado(int id)
        {
            if (id != 0)
            {
                Empleado = cempleados.BuscarxId(id);
                cempleados.Activar(Empleado.Id);
                cempleados.Reload(Empleado);
                GetEmpleados();
                Regresar();
            }
        }

        private void EditarEmpleado(int id)
        {
            if(Empleado != null)
            {
                Empleado.CategoriaId = id;

                Error = "";
                if (cempleados.Validar(Empleado, out List<string> lista))
                {
                    var existe = cempleados.BuscarxId(Empleado.Id);
                    if (existe != null)
                    {
                        existe.Id = Empleado.Id;
                        existe.Nombre = Empleado.Nombre;
                        existe.Sueldo = Empleado.Sueldo;
                        existe.Activo = Empleado.Activo;
                        existe.CategoriaId = Empleado.CategoriaId;
                        existe.Categoria = Empleado.Categoria;
                        cempleados.Update(existe);

                        cempleados.Reload(existe);

                        GetEmpleados();

                        Regresar();
                    }
                }
                else
                {
                    foreach (var error in lista)
                        Error = $"{Error} {error} {Environment.NewLine}";

                    Actualizar();
                }
            }
        }

        private void VerEditarEmpleado(int obj)
        {
            Empleado = cempleados.BuscarxId(obj);
            if (Empleado is not null)
            {

                var clon = new Empleados() {
                Id = Empleado.Id,
                Nombre = Empleado.Nombre,
                Sueldo = Empleado.Sueldo,
                Activo= Empleado.Activo,
                CategoriaId= Empleado.CategoriaId,
                Categoria = Empleado.Categoria
                };
                

                Empleado = clon;
                Vista = Vista.VerEditarEmpleado;
                Actualizar();

            }

        }

        private void VerEmpleadosEliminados()
        {
            Vista = Vista.VerEmpleadosEliminados;
            GetEmpleadosEliminados();
            TotalEmpleadosEliminados = ListaEmpleadosEliminados.Count();
            Actualizar();
        }

        public void GetEmpleadosEliminados()
        {
            ListaEmpleadosEliminados.Clear();
            foreach (Empleados emp in cempleados.GetEmpleadosEliminados())
            {
                ListaEmpleadosEliminados.Add(emp);
            }
        }

        private void Regresar()
        {
            Vista = Vista.VerEmpleados;
            Error = "";
            TotalEmpleados = ListaEmpleados.Count();
            Actualizar();
        }

        private void EliminarEmpleado()
        {
            if(Empleado is not null)
            {
                cempleados.Delete(Empleado.Id);
                cempleados.Reload(Empleado);
                GetEmpleados();
                TotalEmpleados = ListaEmpleados.Count();
                Regresar();
            }
        }

        private void VerEliminarEmpleado(int id)
        {
            Empleado = cempleados.BuscarxId(id);
            if (Empleado is not null)
            {
                Vista = Vista.VerEliminarEmpleado;
                Actualizar();
            }
        }

        private void AgregarEmpleado(int id)
        {
            if (Empleado != null)
            {
                Empleado.CategoriaId = id;
                Error = "";
                if (cempleados.Validar(Empleado, out List<string> lista))
                {
                    cempleados.Create(Empleado);
                    Vista = Vista.VerEmpleados;
                    cempleados.Reload(Empleado);
                    cempleados.Reload(Empleado.CategoriaId);
                    GetEmpleados();
                    TotalEmpleados = ListaEmpleados.Count();

                }
                else
                {
                    foreach (var error in lista)
                        Error = $"{Error} {error} {Environment.NewLine}";

                    Actualizar();
                }
            }
        }

        private void VerAgregarEmpleado()
        {
            Vista = Vista.VerAgregarEmpleado;
            Empleado = new();
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


    }
}
