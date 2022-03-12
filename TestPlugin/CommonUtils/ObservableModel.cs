using System.ComponentModel;


namespace CollisionsManager.CommonUtils
{
    /// <summary>
    /// Represents INotifyPropertyChanged interface and OnPropertyChanged Method
    /// </summary>
    public class ObservableModel : INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName = "")
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
