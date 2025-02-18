using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace ImageViewer.Core
{
    public class TiltWheelHorizontalScroller
    {
        public static readonly DependencyProperty Enable =
            DependencyProperty.RegisterAttached("Enable",
                typeof(bool),
                typeof(TiltWheelHorizontalScroller),
                new UIPropertyMetadata(false, OnEnableChanged));

        private static readonly HashSet<int> Controls = new HashSet<int>();

        public static bool GetEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(Enable);
        }

        public static void SetEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(Enable, value);
        }

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is Control control && GetEnable(d) && Controls.Add(control.GetHashCode()))
                control.MouseEnter += (sender, e) =>
                {
                    var scrollViewer = d.FindChildOfType<ScrollViewer>();
                    if (scrollViewer != null) TiltWheelMouseScrollHelper.ApplyHelper(scrollViewer, d);
                };
        }
    }

    internal static class DependencyObjectExtensions
    {
        public static T? FindChildOfType<T>(this DependencyObject? originalSource) where T : DependencyObject
        {
            var ret = originalSource as T;
            if (originalSource == null || ret != null) return ret;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(originalSource); i++)
            {
                var child = VisualTreeHelper.GetChild(originalSource, i);
                if (child is T dependencyObject)
                {
                    ret = dependencyObject;
                    break;
                }

                ret = child.FindChildOfType<T>();
                if (ret != null) break;
            }

            return ret;
        }
    }

    internal static class TiltWheelMouseScrollHelper
    {
        private const double ScrollFactor = 2;

        private static readonly HashSet<int> ScrollViewers = new HashSet<int>();

        public static void ApplyHelper(ScrollViewer scrollViewer, DependencyObject d)
        {
            var handler = new TiltHandler(scrollViewer);

            var source = PresentationSource.FromDependencyObject(d) as HwndSource;

            HwndSourceHook hook = handler.Handle;
            source?.AddHook(hook);

            if (ScrollViewers.Add(scrollViewer.GetHashCode()))
                scrollViewer.MouseLeave += (sender, e) => { source?.RemoveHook(hook); };
        }

        private class TiltHandler
        {
            private const int WM_MOUSEHWHEEL = 0x020E;
            private readonly ScrollViewer _scrollViewer;

            public TiltHandler(ScrollViewer scrollViewer)
            {
                _scrollViewer = scrollViewer;
            }

            public IntPtr Handle(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                switch (msg)
                {
                    case WM_MOUSEHWHEEL:
                        Scroll(HIWORD(wParam));
                        handled = true;
                        break;
                }

                return IntPtr.Zero;
            }

            private void Scroll(int delta)
            {
                _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset
                                                       + delta / ScrollFactor);
            }

            private static int HIWORD(IntPtr ptr)
            {
                return (short)(((int)ptr.ToInt64() >> 16) & 0xFFFF);
            }
        }
    }
}