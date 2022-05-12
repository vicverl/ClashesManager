using System.Collections.ObjectModel;
using Autodesk.Revit.UI;
using AutoUpdater;
using ClashesManager.Utils;
using ClashesManager.ViewModels.Utils;
using System.Windows;
using System.Windows.Input;
using ClashesManager.Models;
using ClashesManager.Views;
using ClashesManager.IExternalEvent;

namespace ClashesManager.ViewModels
{
    public class ClashesManagerViewModel : ViewModelBase, IViewModel
    {
        Updater _updater;
        Window _window;
        IClashesManagerModel _model;
        private ICommand _openFileCommand;
        private ICommand _findClashCommand;
        private ICommand _closeCommand;
        private ICommand _removeAllClashTestsCommand;
        private ICommand _removeClashTestCommand;

        public ICommand OpenFileCommand => _openFileCommand ??= new RelayCommand(param => OpenFile());
        public ICommand FindClashCommand => new RelayCommand(param => FindClash());
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(param => Close());
        public ICommand RemoveAllClashTestsCommand => _removeAllClashTestsCommand ??= new RelayCommand(param => ClearAllInformation());
        public ICommand RemoveClashTestCommand => _removeClashTestCommand ??= new RelayCommand(param => RemoveClashTest());

        public ObservableCollection<ClashTestModel> ClashTestsCollection => _model.ClashTestsCollection;

        public ClashTestModel SelectedTest
        {
            get => _model.SelectedTest;
            set => _model.SelectedTest = value;
        }

        public ObservableCollection<ClashModel> ClashesTable
        {
            get => _model.ClashesTable;
            set => _model.ClashesTable = value;
        }

        public ObservableCollection<ClashTestModel> ClashTestTable
        {
            get => _model.ClashTestTable;
            set => _model.ClashTestTable = value;
        }

        public ClashModel SelectedClashModel
        {
            get => _model.SelectedClashModel;
            set => _model.SelectedClashModel = value;
        }

        public string XmlPath
        {
            get => _model.XmlPath;
            set => _model.XmlPath = value;
        }

        public void Initialize(UIApplication uiApp, Window window)
        {
            _model = new ClashesManagerModel();
            _model.LoadSettings();

            _window = window;
            _window.Closing += OnWindow_Closing;
            _model.ClashTestsCollectionUpdated += ClashTestsCollectionUpdated;
            _model.SelectedClashTestUpdated += SelectedClashTestUpdated;
            _model.ClashesTableUpdated += ClashesTableUpdated;
            _model.ClashTestTableUpdated += ClashTestTableUpdated;
            _model.SelectedClashModelUpdated += SelectedClashModelUpdated;
            _model.XmlPathUpdated += XmlPathUpdated;

            if (ExternalApplication.ThisApp != null)
            {
                _updater = new Updater(new UpdatingApplication());
                _updater.CheckUpdates(_window);
            }
        }

        private void OnWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // сохраняем путь к инсталлятору
            if (ExternalApplication.ThisApp != null)
                ExternalApplication.PathOfDownloadedInstaller = _updater.PathOfDownloadeedInstaller;
        }

        private void OpenFile()
        {
            _model.OpenFile();
        }

        private void Close()
        {
            AppSettingsUtil.SaveSettings(ClashesManagerView.Settings); // Saving all xml paths
            _model.Serialize();
            _window.Close();
            if (ExternalApplication.ThisApp != null)
                ExternalApplication.showButton.Enabled = true;
        }

        private void FindClash()
        {
            MessageBox.Show("Hello");
        }

        /// <summary>
        /// Delete all clash tests from interface
        /// </summary>
        private void ClearAllInformation()
        {
            _model.ClearAllInformation();
        }

        private void RemoveClashTest()
        {
            MessageBox.Show("Is not working yet", "Oops");
        }

        public void UpdateSelectedClashModel(ClashModel selectedClashModel)
        {
            _model.UpdateSelectedClashModel(selectedClashModel);
        }

        private void ClashTestsCollectionUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ClashTestsCollection));
        }

        private void SelectedClashTestUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedTest));
        }

        private void ClashesTableUpdated(object sender, EventArgs e) 
        {
            OnPropertyChanged(nameof(ClashesTable));
        }

        private void ClashTestTableUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ClashTestTable));
        }

        private void SelectedClashModelUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedClashModel));
        }
        private void XmlPathUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(XmlPath));
        }
    }
}