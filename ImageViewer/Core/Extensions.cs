using System.Windows;

namespace ImageViewer.Core
{
    public class Extensions
    {
        public static readonly DependencyProperty Extra =
            DependencyProperty.RegisterAttached("Extra",
                typeof(string),
                typeof(Extensions),
                new PropertyMetadata(default(string)));
        
        public static void SetExtra(DependencyObject obj, string value) => obj.SetValue(Extra, value);

        public static string GetExtra(DependencyObject obj) => (string)obj.GetValue(Extra);
    }
}