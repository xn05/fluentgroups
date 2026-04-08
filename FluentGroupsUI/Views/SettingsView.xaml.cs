using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace FluentGroupsUI.Views;

public sealed partial class SettingsView : UserControl
{
    private bool _isUpdatingState;

    public event EventHandler<bool>? DarkThemeToggled;

    public SettingsView()
    {
        this.InitializeComponent();
    }

    public void SetDarkThemeState(bool isDarkTheme)
    {
        _isUpdatingState = true;
        DarkThemeToggle.IsOn = isDarkTheme;
        _isUpdatingState = false;
    }

    private void DarkThemeToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (_isUpdatingState)
        {
            return;
        }

        DarkThemeToggled?.Invoke(this, DarkThemeToggle.IsOn);
    }
}

