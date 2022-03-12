using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionsManager.CommonUtils;
using TestPlugin.Models;

namespace CollisionsManager.ViewModels
{
    class ViewModel : ObservableModel
    {
        #region Fields

        #endregion
        #region Properties
        private string _firstId = "14";

        public string FirstId
        {
            private get { return Model.FirstId; }
            set 
            {
                Model.FirstId = value;
            }
        }

        private string _secondId = "15";

        public string SecondId
        {
            get { return Model.SecondId; }
            set
            {
                Model.SecondId = value;
            }
        }

        private IModel Model { get; set; }
        #endregion

        #region Methods
        public void Initialize()
        {
            IModel iModel = new Model();
            Model = iModel;
        }
        #endregion
        #region Events

        #endregion
        #region Constructors
        public ViewModel()
        {
        }
        #endregion
    }
}
