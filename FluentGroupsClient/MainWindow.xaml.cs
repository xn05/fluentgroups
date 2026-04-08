using System;
using System.Runtime.InteropServices;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Windows.Graphics;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using FluentGroupsUI.Views;
using FluentGroupsServices;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentGroupsClient
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private ThemeService? _themeService;
        
        private const int BaseWidthDip = 550;
        private const int BaseHeightDip = 760;
        private const double DpiBase = 96d;
        
        private double _lastAppliedScale = -1d;

        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            ResizeForCurrentScale(force: true);
            AppWindow.Changed += AppWindow_Changed;
            
            if (AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
            }
            
            try
            {
                // Initialize theme service
                _themeService = new ThemeService();
                _themeService.Initialize();
                _themeService.AttachThemeRoot(WindowRoot);
                
                // Apply initial theme to window chrome/buttons.
                ApplyWindowChromeTheme(_themeService.CurrentTheme);
                
                // Subscribe to theme changes
                _themeService.ThemeChanged += (sender, theme) =>
                {
                    ApplyWindowChromeTheme(theme);
                };
                
                // Create shell with theme service
                var shell = new FluentGroupsShell(_themeService);
                RootGrid.Children.Add(shell);
            }
            catch (Exception ex)
            {
                RootGrid.Children.Add(new ScrollViewer
                {
                    Content = new TextBlock
                    {
                        Text = $"UI failed to initialize:\n{ex}",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(16)
                    }
                });
            }
        }
        
        private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
        {
            ResizeForCurrentScale();
        }
        
        private void ResizeForCurrentScale(bool force = false)
        {
            var hwnd = WindowNative.GetWindowHandle(this);
            if (hwnd == IntPtr.Zero)
            {
                return;
            }

            var dpi = GetDpiForWindow(hwnd);
            if (dpi == 0)
            {
                return;
            }

            var scale = dpi / DpiBase;
            if (!force && Math.Abs(scale - _lastAppliedScale) < 0.01d)
            {
                return;
            }

            _lastAppliedScale = scale;

            var targetSize = new SizeInt32(
                (int)Math.Ceiling(BaseWidthDip * scale),
                (int)Math.Ceiling(BaseHeightDip * scale));

            if (AppWindow.Size.Width == targetSize.Width && AppWindow.Size.Height == targetSize.Height)
            {
                return;
            }

            AppWindow.Resize(targetSize);
        }

        private void ApplyWindowChromeTheme(ElementTheme theme)
        {
            WindowRoot.RequestedTheme = theme;

            var titleBar = AppWindow.TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            if (theme == ElementTheme.Dark)
            {
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonInactiveForegroundColor = ColorHelper.FromArgb(255, 180, 180, 180);
                titleBar.ButtonHoverBackgroundColor = ColorHelper.FromArgb(40, 255, 255, 255);
                titleBar.ButtonPressedBackgroundColor = ColorHelper.FromArgb(80, 255, 255, 255);
                return;
            }

            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonInactiveForegroundColor = ColorHelper.FromArgb(255, 90, 90, 90);
            titleBar.ButtonHoverBackgroundColor = ColorHelper.FromArgb(20, 0, 0, 0);
            titleBar.ButtonPressedBackgroundColor = ColorHelper.FromArgb(40, 0, 0, 0);
        }

        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hwnd);
    }
}
