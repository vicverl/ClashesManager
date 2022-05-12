using System.ComponentModel;

namespace ClashesManager
{
    [TypeConverter(typeof(EnumOwnConverter))]
    public enum StatusEnum
    {
        [Description("Новая")]
        New,
        [Description("В работе")]
        InProcess,
        [Description("Исправлена")]
        Fixed,
        [Description("Другой раздел")]
        Adjacent
    }
}
