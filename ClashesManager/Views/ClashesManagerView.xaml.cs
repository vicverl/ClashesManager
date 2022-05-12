using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autodesk.Revit.UI;
using ClashesManager.IExternalEvent;
using ClashesManager.Models;
using ClashesManager.Utils;
using ClashesManager.ViewModels;
using ClashesManager.ViewModels.Utils;

namespace ClashesManager.Views
{
    public partial class ClashesManagerView : Window
    {
        private ClashesManagerViewModel _viewModel;
        SearchEvent showElementOn3D;
        Autodesk.Revit.UI.ExternalEvent showElementOn3DEvent;
        public static Window CurrentWindow { get; private set; }
        public static AppSettings Settings { get; private set; }
        ExternalEvent searchEventRevit;
        public ClashesManagerView(UIApplication uiApp)
        {
            showElementOn3D = new SearchEvent(this);
            showElementOn3DEvent = Autodesk.Revit.UI.ExternalEvent.Create(showElementOn3D);
            Settings = AppSettingsUtil.LoadSettings();
            CurrentWindow = this;
            _viewModel = new ClashesManagerViewModel();
            _viewModel.Initialize(uiApp, this);
            DataContext = _viewModel;

            InitializeComponent();
        }
        
        private void Hide_BTN_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        
        private void Window_TopPart_CNVS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public void UpdateSelectedClashModel(object sender, SelectedCellsChangedEventArgs e)
        {
            var dataGrid = sender as System.Windows.Controls.DataGrid;
            if (dataGrid?.CurrentItem is ClashModel selectedClashModel)
                _viewModel.UpdateSelectedClashModel(selectedClashModel);
        }

        private void WindowMaximize_BTN_Click(object sender, RoutedEventArgs e)
        {
            WindowStateIconChanger();
        }

        private void WindowStateIconChanger()
        {
            WindowState = WindowState switch
            {
                WindowState.Maximized => WindowState.Normal,
                WindowState.Normal => WindowState.Maximized,
                _ => WindowState
            };
        }

        private void OpenFolder_BTN_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ClashTest_DGrid.SelectedItem as ClashModel;
            showElementOn3D.FirstId = Convert.ToInt32(selectedItem.FirstElementId);
            showElementOn3D.SecondId = Convert.ToInt32(selectedItem.SecondElementId);
            showElementOn3D.ElelmDocNameFisrt = selectedItem.FirstElementDoc.Replace(".NWC", "").Replace(".nwc", "");
            showElementOn3D.ElelmDocNameSecond = selectedItem.SecondElementDoc.Split('.')[0];
            showElementOn3DEvent.Raise();
        }
    }
}