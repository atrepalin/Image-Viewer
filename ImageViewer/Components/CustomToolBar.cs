using System.Windows;
using System.Windows.Controls;

namespace ImageViewer.Components
{
    public class CustomToolBar : ToolBar
    {
        public CustomToolBar()
        {
            Loaded += (sender, args) =>
            {
                if (Template.FindName("OverflowGrid", this) is FrameworkElement overflowGrid)
                    overflowGrid.Visibility = Visibility.Collapsed;

                if (Template.FindName("MainPanelBorder", this) is FrameworkElement mainPanelBorder)
                    mainPanelBorder.Margin = new Thickness();
            };
        }
    }
}