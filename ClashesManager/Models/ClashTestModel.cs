using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClashesManager.Models
{
    [Serializable]
    public class ClashTestModel
    {
        private string _testName;
        public string TestName
        {
            get => _testName;
            set => _testName = value;
        }

        private string _testType;
        public string TestType
        {
            get => _testType;
            set => _testType = value;
        }

        private string _tolerance;
        public string Tolerance
        {
            get => _tolerance;
            set => _tolerance = value;
        }

        private string _clashTestDescription;
        public string ClashTestDescription
        {
            get => _clashTestDescription;
            set => _clashTestDescription = value;
        }

        private ObservableCollection<ClashModel> _clashModels = new();
        public ObservableCollection<ClashModel> ClashModels
        {
            get => _clashModels; 
            set => _clashModels = value;
        }

        private string _xmlFilePath;
        public string XmlFilePath
        {
            get => _xmlFilePath;
            set => _xmlFilePath = value;
        }
    }
}
