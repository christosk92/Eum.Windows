using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace Eum.UI.WinUI;

public class ViewLocator : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is { } routableViewModel)
        {
            var itemType = routableViewModel.GetType();


            var name = itemType.FullName!.Replace("ViewModel", "View")
                .Replace("Eum.UI", "Eum.UI.WinUI");
            var type = Type.GetType(name);

            if (type == null)
            {
                //check base type
                name = itemType.BaseType.FullName!.Replace("ViewModel", "View")
                    .Replace("Eum.UI", "Eum.UI.WinUI");
                type = Type.GetType(name);
            }
            if (type != null && type != typeof(object))
            {

                //return (Control)Activator.CreateInstance(type)!;
                
                return (Control)Activator.CreateInstance(type, value)!;

            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}