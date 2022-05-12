using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ClashesManager.Core;
using ClashesManager.Utils;
using ClashesManager.Views;
using static System.String;


namespace ClashesManager.Models
{
    internal class ClashesManagerModel : IClashesManagerModel
    {
        private string SerializedXmlPath { get; set; }

        private Dictionary<string ,List<ClashTestModel>> _clashTestsDictionary = new();
        public Dictionary<string, List<ClashTestModel>> ClashTestsDictionary
        {
            get => _clashTestsDictionary;
            set => _clashTestsDictionary = value;
        }

        private ObservableCollection<ClashTestModel> _clashTestsCollection = new();
        public ObservableCollection<ClashTestModel> ClashTestsCollection
        {
            get => _clashTestsCollection;
            set
            {
                _clashTestsCollection = value;
                ClashTestsCollectionUpdated(this, new EventArgs());
            }
        }

        private string _xmlPath;
        public string XmlPath
        {
            get => _xmlPath;
            set
            {
                _xmlPath = value;
                XmlPathUpdated(this, new EventArgs());
            }
        }

        private ClashTestModel _selectedTest;
        public ClashTestModel SelectedTest
        {
            get => _selectedTest;
            set
            {
                _selectedTest = value;
                ClashesManagerView.Settings.LastOpenedTest = value;
                SelectedClashTestUpdated(this, new EventArgs());

                if (value is null) return;
                ClashesTable = _selectedTest.ClashModels;
                ClashTestTable = new ObservableCollection<ClashTestModel> { _selectedTest };

                XmlPath = value.XmlFilePath;
            }
        }

        private ObservableCollection<ClashTestModel> _clashTestTable;
        public ObservableCollection<ClashTestModel> ClashTestTable
        {
            get => _clashTestTable;
            set
            {
                _clashTestTable = value;
                ClashTestTableUpdated(this, new EventArgs());
            } 
        }

        private ObservableCollection<ClashModel> _clashesTable;
        public ObservableCollection<ClashModel> ClashesTable
        {
            get => _clashesTable;
            set
            {
                _clashesTable = value;
                ClashesTableUpdated(this, new EventArgs());
            }
        }

        // A collection to fill element1 and element2 datagrids
        private ClashModel _selectedClashModel;
        public ClashModel SelectedClashModel
        {
            get => _selectedClashModel;
            set
            {
                _selectedClashModel = value;
                ClashesManagerView.Settings.LastOpenedClash = value;
                SelectedClashModelUpdated(this, new EventArgs());
            }
        }

        public void UpdateSelectedClashModel(ClashModel selectedClashModel)
        {
            SelectedClashModel = selectedClashModel;
        }

        public void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            
            try
            {
                var serializedXmlPath = XmlUtils.CreateNewPath(openFileDialog.FileName); // Creates a new path to serialized Xml file
                if (serializedXmlPath is null) return;

                // Checks if Xml path is already in history and adds if not
                if (ClashesManagerView.Settings.XmlPathCollection.Contains(serializedXmlPath))
                {
                    var rewriteView = new RewriteView();
                    if (!rewriteView.ViewModel.ToRewrite) return;
                    var clashTestsToAdd = XmlUtils.CreateClashTestsList(openFileDialog.FileName); // Parses existed Xml file and creates data

                    // Creating a list of tests with the same name as new tests
                    // then delete them to clear the list of tests that will be shown in combobox
                    var testsToDelete = (from loadedTest in ClashTestsCollection from newTest in clashTestsToAdd where loadedTest.TestName == newTest.TestName select loadedTest).ToList();

                    foreach (var test in testsToDelete)
                        ClashTestsCollection.Remove(test);

                    // Updating dictionary that will be used in serialization
                    if (ClashTestsDictionary.ContainsKey(serializedXmlPath))
                        ClashTestsDictionary.Remove(serializedXmlPath);
                    ClashTestsDictionary.Add(serializedXmlPath, clashTestsToAdd);

                    // Updating of collection that will be shown in combobox
                    foreach (var clashTest in clashTestsToAdd)
                        ClashTestsCollection.Add(clashTest);
                    
                    ClashesTable = clashTestsToAdd.FirstOrDefault()?.ClashModels;
                }
                // if file is not in the history list
                else
                {
                    var clashTestsToAdd = new List<ClashTestModel>();

                    if (File.Exists(serializedXmlPath))
                    {
                        clashTestsToAdd = DeserializeFile(serializedXmlPath); // Load data from file
                        
                        if (clashTestsToAdd.Count == 0)
                        {
                            clashTestsToAdd = XmlUtils.CreateClashTestsList(openFileDialog.FileName); // Parses existed Xml file and creates data
                     
                            foreach (var clashTest in clashTestsToAdd)
                                ClashTestsCollection.Add(clashTest);

                            ClashesManagerView.Settings.XmlPathCollection.Add(serializedXmlPath);
                            return;
                        }

                        ClashesManagerView.Settings.XmlPathCollection.Add(serializedXmlPath);
                        foreach (var clashTest in clashTestsToAdd)
                            ClashTestsCollection.Add(clashTest);
                    }
                    else
                    {
                        ClashesManagerView.Settings.XmlPathCollection.Add(serializedXmlPath);
                        clashTestsToAdd = XmlUtils.CreateClashTestsList(openFileDialog.FileName); // Parses existed Xml file and creates data
                        ClashTestsDictionary.Add(serializedXmlPath, clashTestsToAdd);
                        foreach (var clashTest in clashTestsToAdd)
                            ClashTestsCollection.Add(clashTest);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Analytics.SaveExceptionReport(ex, "Проблемы при считывании файла xml");
            }   
        }

        public void Serialize()
        {
            try
            {
                if (ClashesManagerView.Settings.XmlPathCollection.Count > 0 & ClashTestsCollection.Count > 0)
                {
                    foreach (var clashTestDictionary in ClashTestsDictionary)
                    {
                        var xmlPath = clashTestDictionary.Key;
                        var formatter = new XmlSerializer(typeof(List<ClashTestModel>));

                        using (var writer = XmlWriter.Create(xmlPath))
                        {
                            formatter.Serialize(writer, clashTestDictionary.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении настроек. Обратитесь к разработчику плагина", "Ошибка!");
                Analytics.SaveExceptionReport(ex, "Ошибка при сохранении настроек.");
            }
        }

        public void LoadSettings()
        {
            if (ClashesManagerView.Settings is null) return;

            var xmlPathsCollection = ClashesManagerView.Settings.XmlPathCollection;
            if (xmlPathsCollection.Count == 0) return;

            var notDeserializedXmlColletion = new List<string>();

            // Adding clash tests from AppSettings to UI
            foreach (var xmlPath in xmlPathsCollection)
            {
                if (!File.Exists(xmlPath))
                {
                    notDeserializedXmlColletion.Add(xmlPath);
                    continue;
                }
                var testCollection = DeserializeFile(xmlPath);

                if (testCollection is null)
                    notDeserializedXmlColletion.Add(xmlPath);
                else
                {
                    foreach (var clashTest in testCollection)
                        ClashTestsCollection.Add(clashTest);
                }
            }

            foreach (var xmlPath in notDeserializedXmlColletion)
                ClashesManagerView.Settings.XmlPathCollection.Remove(xmlPath);
            

            var notLoadedXmlfiles = "";

            if (notDeserializedXmlColletion.Any())
            {
                foreach (var xmlPath in notDeserializedXmlColletion)
                {
                    ClashesManagerView.Settings.XmlPathCollection.Remove(xmlPath);
                    var fileName = xmlPath.Split('\\')
                        .Last()
                        .Replace("REPORT-", "");

                    notLoadedXmlfiles = notLoadedXmlfiles+ "\n" + fileName;
                }

                MessageBox.Show($"Ошибка при загрузке следующих xml файлов:{notLoadedXmlfiles}", "Ошибка!");
            }
            
            // Setting last opened clash test to be selected
            var lastClashTestOpened = ClashTestsCollection.FirstOrDefault(clashTest => clashTest.TestName == ClashesManagerView.Settings.LastOpenedTest.TestName &
                                                             clashTest.XmlFilePath == ClashesManagerView.Settings.LastOpenedTest.XmlFilePath);
            SelectedTest = lastClashTestOpened ?? ClashTestsCollection?.FirstOrDefault();
            // Setting last opened clash to be selected
            var lastClashOpened = ClashesTable.FirstOrDefault(clash => clash.ClashName == ClashesManagerView.Settings.LastOpenedClash?.ClashName);
            SelectedClashModel = lastClashOpened;
        }

        [CanBeNull]
        private List<ClashTestModel> DeserializeFile(string xmlPath)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(List<ClashTestModel>));
                using (var reader = XmlReader.Create(xmlPath))
                {
                    var clastTestsCollection = xmlSerializer.Deserialize(reader) as List<ClashTestModel>;
                    ClashTestsDictionary.Add(xmlPath, clastTestsCollection);
                    return clastTestsCollection;
                }
            }
            catch
            {
                return null;
            }
        }

        public void ClearAllInformation()
        {
            ClashTestsDictionary = new Dictionary<string, List<ClashTestModel>>();
            ClashTestsCollection = new ObservableCollection<ClashTestModel>(); // Remove all tests
            ClashesTable = new ObservableCollection<ClashModel>(); // Reset clashes dataGrid
            ClashTestTable = new ObservableCollection<ClashTestModel>(); // Reset clashTests dataGrid
            ClashesManagerView.Settings.XmlPathCollection = new List<string>(); // Reset all xml paths from settings not to load it in the next time
            SelectedClashModel = null;
            XmlPath = null;
        }

        public event EventHandler<EventArgs> ClashTestsCollectionUpdated = delegate { };
        public event EventHandler<EventArgs> SelectedClashTestUpdated = delegate { };
        public event EventHandler<EventArgs> ClashesTableUpdated = delegate { };
        public event EventHandler<EventArgs> ClashTestTableUpdated = delegate { };
        public event EventHandler<EventArgs> SelectedClashModelUpdated = delegate { };
        public event EventHandler<EventArgs> XmlPathUpdated = delegate { };
    }
}
