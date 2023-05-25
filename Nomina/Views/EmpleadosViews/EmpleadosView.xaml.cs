using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Win32;

namespace Nomina.Views.EmpleadosViews
{
    /// <summary>
    /// Lógica de interacción para EmpleadosView.xaml
    /// </summary>
    public partial class EmpleadosView : UserControl
    {
        public EmpleadosView()
        {
            InitializeComponent();
        }

        private void btnExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog ventana = new OpenFileDialog();

            //if (ventana.ShowDialog() == true)
            //{
            //    txtRutaExecel.Text = ventana.;
            //}


        }
        private bool Mostrar = false;
        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            if (Mostrar == false)
            {
                MenuFiltrar.Visibility = Visibility.Visible;
                Mostrar = true;
            }
            else
            {
                Mostrar = false;
                MenuFiltrar.Visibility = Visibility.Collapsed;

            }

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuFiltrar.Visibility = Visibility.Collapsed;
        }

        private void CerrarFiltrar_Click(object sender, RoutedEventArgs e)
        {
            MenuFiltrar.Visibility = Visibility.Collapsed;
        }
    }
}
