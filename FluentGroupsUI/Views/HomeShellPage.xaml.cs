using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace FluentGroupsUI.Views;

public sealed partial class HomeShellPage : Page
{
    public event EventHandler? SettingsRequested;
    public event EventHandler? NewGroupRequested;

    public HomeShellPage()
    {
        this.InitializeComponent();
        NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Required;
    }

    private void NewGroupButton_Click(object sender, RoutedEventArgs e)
    {
        NewGroupRequested?.Invoke(this, EventArgs.Empty);
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        SettingsRequested?.Invoke(this, EventArgs.Empty);
    }
}
