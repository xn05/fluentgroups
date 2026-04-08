using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FluentGroupsServices;
using System;

namespace FluentGroupsUI.Views;

public sealed partial class SettingsPage : Page
{
    public event EventHandler<bool>? DarkThemeToggled;

    public SettingsPage()
    {
        this.InitializeComponent();
        NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Required;
        SettingsViewControl.DarkThemeToggled += (sender, isDarkTheme) => DarkThemeToggled?.Invoke(sender, isDarkTheme);
    }

    public void SetDarkThemeState(bool isDarkTheme)
    {
        SettingsViewControl.SetDarkThemeState(isDarkTheme);
    }

    public void ApplyTheme(ElementTheme theme)
    {
        SettingsViewControl.RequestedTheme = theme;
    }
}

