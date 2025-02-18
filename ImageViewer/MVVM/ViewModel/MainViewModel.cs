using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageViewer.Core;
using Microsoft.Win32;

namespace ImageViewer.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private RelayCommand? _clearImage;

        private RelayCommand? _closeWindow;

        private ImageSource? _imgSource;

        private RelayCommand? _maximizeWindow;

        private RelayCommand? _minimizeWindow;

        private RelayCommand? _moveWindow;

        private RelayCommand? _openImage;

        private ImageSource? _originalSource;

        private int _selectedSize = 100;

        public int SelectedSize
        {
            get => _selectedSize;
            set
            {
                if (_originalSource != null) ImgSource = _originalSource.CreateResizedImage(value / 100d);

                _selectedSize = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OpenImageCommand
        {
            get
            {
                return _openImage ?? new RelayCommand(_ =>
                {
                    var openFileDialog = new OpenFileDialog
                    {
                        Filter = "Файлы изображений (*.bmp, *.jpg, *.png, *.webp)|*.bmp;*.jpg;*.jpeg;*.png;*.webp" +
                                 "|Все файлы (*.*)|*.*"
                    };

                    if (openFileDialog.ShowDialog() != true) return;

                    var file = openFileDialog.FileName;

                    try
                    {
                        ImgSource = _originalSource = new BitmapImage(new Uri(file, UriKind.Absolute));
                    }
                    catch
                    {
                        ImgSource = _originalSource = null;
                    }

                    SelectedSize = 100;
                });
            }
        }

        public RelayCommand ClearImageCommand
        {
            get
            {
                return _clearImage ?? new RelayCommand(_ =>
                {
                    ImgSource = _originalSource = null;

                    SelectedSize = 100;
                });
            }
        }

        public RelayCommand MoveWindowCommand
        {
            get { return _moveWindow ??= new RelayCommand(_ => { Application.Current.MainWindow!.DragMove(); }); }
        }

        public RelayCommand MinimizeWindowCommand
        {
            get
            {
                return _minimizeWindow ??= new RelayCommand(_ =>
                {
                    Application.Current.MainWindow!.WindowState = WindowState.Minimized;
                });
            }
        }

        public RelayCommand MaximizeWindowCommand
        {
            get
            {
                return _maximizeWindow ??= new RelayCommand(_ =>
                {
                    Application.Current.MainWindow!.WindowState =
                        Application.Current.MainWindow!.WindowState == WindowState.Maximized
                            ? WindowState.Normal
                            : WindowState.Maximized;
                });
            }
        }

        public RelayCommand CloseWindowCommand
        {
            get { return _closeWindow ??= new RelayCommand(_ => { Environment.Exit(0); }); }
        }

        public ImageSource? ImgSource
        {
            get => _imgSource;
            set
            {
                _imgSource = value;
                OnPropertyChanged();
            }
        }
    }
}