using System.ComponentModel;

namespace Khuyaway.Common;

public static class EnumExtensions
{
    public static string GetTitleDescription(this Enum value)
    {
        var type = value.GetType();
        var description = (DescriptionAttribute)Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute));
        return description?.Description ?? value.ToString();
    }

    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var description = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
        return description?.Description ?? value.ToString();
    }
}