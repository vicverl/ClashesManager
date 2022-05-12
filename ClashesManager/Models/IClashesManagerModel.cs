using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashesManager.Models
{
    internal interface IClashesManagerModel
    {
        ObservableCollection<ClashTestModel> ClashTestsCollection { get; set; }
        ClashTestModel SelectedTest { get; set; }
        ObservableCollection<ClashModel> ClashesTable { get; set; }
        ObservableCollection<ClashTestModel> ClashTestTable { get; set; }
        ClashModel SelectedClashModel { get; set; }
        string XmlPath { get; set; }

        void OpenFile();
        void Serialize();
        void UpdateSelectedClashModel(ClashModel selectedClashModel);
        void LoadSettings();
        void ClearAllInformation();

        event EventHandler<EventArgs> ClashTestsCollectionUpdated;
        event EventHandler<EventArgs> SelectedClashTestUpdated;
        event EventHandler<EventArgs> ClashesTableUpdated;
        event EventHandler<EventArgs> ClashTestTableUpdated;
        event EventHandler<EventArgs> SelectedClashModelUpdated;
        event EventHandler<EventArgs> XmlPathUpdated;
    }
}
