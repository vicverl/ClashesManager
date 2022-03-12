using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin.Models
{
    class Model : IModel
    {
        #region Fields

        #endregion
        #region Properties
        private string _firstId = "14";

        public string FirstId
        {
            get { return _firstId; }
            set { _firstId = value; }
        }

        private string _secondId = "15";

        public string SecondId
        {
            get { return _secondId; }
            set { _secondId = value; }
        }
        #endregion
        #region Methods
        public void Initialize()
        {

        }
        #endregion
        #region Events

        #endregion
        #region Constructors
        public Model()
        {

        }
        #endregion
    }
}
