using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashesManager.Models
{
    public class ClashModel
    {
        private string _number;
        public string Number
        {
            get => _number;
            set => _number = value;
        }

        private string _note;
        public string Note
        {
            get => _note;
            set => _note = value;
        }

        private string _clashName;
        public string ClashName
        {
            get => _clashName;
            set => _clashName = value;
        }

        private string _clashGridLocation;

        public string ClashGridLocation
        {
            get => _clashGridLocation;
            set => _clashGridLocation = value;
        }

        private string _firstElementId;
        public string FirstElementId
        {
            get => _firstElementId;
            set => _firstElementId = value;
        }

        private string _secondElementId;
        public string SecondElementId
        {
            get => _secondElementId;
            set => _secondElementId = value;
        }

        private string _firstElementDoc;
        public string FirstElementDoc
        {
            get => _firstElementDoc;
            set => _firstElementDoc = value;
        }

        private string _secondElementDoc;
        public string SecondElementDoc
        {
            get => _secondElementDoc;
            set => _secondElementDoc = value;
        }

        private string _firstElementName;
        public string FirstElementName
        {
            get => _firstElementName;
            set => _firstElementName = value;
        }

        private string _secondElementName;
        public string SecondElementName
        {
            get => _secondElementName;
            set => _secondElementName = value;
        }

        private string _firstElementType;
        public string FirstElementType
        {
            get => _firstElementType;
            set => _firstElementType = value;
        }

        private string _secondElementType;
        public string SecondElementType
        {
            get => _secondElementType;
            set => _secondElementType = value;
        }

        private StatusEnum _status;
        public StatusEnum Status
        {
            get => _status;
            set => _status = value;
        }

        private string _imgPath;
        public string ImgPath
        {
            get => _imgPath;
            set => _imgPath = value;
        }
    }
}
