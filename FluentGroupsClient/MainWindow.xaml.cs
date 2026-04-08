using System;
using Microsoft.UI.Xaml;
using Windows.Graphics;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using FluentGroupsUI.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentGroupsClient
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            AppWindow.Resize(new SizeInt32(520, 760));
            
            if (AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
            }
            
            try
            {
                RootGrid.Children.Add(new HomeShellPage());
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
    }
}
