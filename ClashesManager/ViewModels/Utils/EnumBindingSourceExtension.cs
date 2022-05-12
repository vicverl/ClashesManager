using ClashesManager.Models;
using ClashesManager.Utils;
using System.Windows.Markup;

namespace ClashesManager
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;

        public EnumBindingSourceExtension(Type enumType)
        {
            _enumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(_enumType);
            StatusEnum[] arrayDesc = new StatusEnum[enumValues.Length];
            try
            {
                for (int i = 0; i < enumValues.Length; i++)
                {
                    arrayDesc[i] = ((StatusEnum[])enumValues)[i];
                }
            }
            catch (Exception ex)
            {
                Analytics.SaveExceptionReport(ex, "ProvideValue EnumBindingSourceExtension Error");
            }
            return arrayDesc;
        }
    }
}