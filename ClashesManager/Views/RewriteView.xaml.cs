using System.Windows;
using ClashesManager.ViewModels;

namespace ClashesManager.Views
{
    /// <summary>
    /// Логика взаимодействия для RewriteView.xaml
    /// </summary>
    public partial class RewriteView : Window
    {
        public RewriteViewModel ViewModel { get; }
        public RewriteView()
        {
            ViewModel = new RewriteViewModel();
            ViewModel.Initialize(this);
            DataContext = ViewModel;
            InitializeComponent();
            ShowDialog();
        }
    }
}
