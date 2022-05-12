using ClashesManager.ViewModels.Utils;

namespace ClashesManager.Models
{
    public class AppSettings : ViewModelBase
    {
        private List<string> _xmlPathCollection = new();
        public List<string> XmlPathCollection
        {
            get => _xmlPathCollection;
            set => _xmlPathCollection = value;
        }

        private ClashTestModel _lastOpenedTest;
        public ClashTestModel LastOpenedTest
        {
            get => _lastOpenedTest;
            set => _lastOpenedTest = value;
        }

        private ClashModel _lastOpenedClash;
        public ClashModel LastOpenedClash
        {
            get => _lastOpenedClash;
            set => _lastOpenedClash = value;
        }
    }
}
