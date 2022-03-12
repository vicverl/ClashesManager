using Autodesk.Revit.UI;
using CollisionsManager.ViewModels;
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

namespace CollisionsManager
{
    public partial class UI : Window
    {
        public static Window CurrentWindow { get; private set; }

        public UI()
        {
        }

        public UI(UIApplication uiApp)
        {
            CurrentWindow = this;
            ViewModel viewModel = new ViewModel();
            viewModel.Initialize();
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void Hide_BTN_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_BTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //перетаскивание нажатием на шапку плагина
        private void Window_TopPart_CNVS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
