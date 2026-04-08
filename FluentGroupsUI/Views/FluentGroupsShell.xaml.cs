using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using FluentGroupsServices;
using System;

namespace FluentGroupsUI.Views;

public sealed partial class FluentGroupsShell : UserControl
{
    private readonly ThemeService _themeService;

    public FluentGroupsShell(ThemeService themeService)
    {
        this.InitializeComponent();
        _themeService = themeService;
        
        // Subscribe to theme changes
        _themeService.ThemeChanged += OnThemeChanged;
        
        // Load the home page by default
        NavigateToHome();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (PageFrame.CanGoBack)
        {
            PageFrame.GoBack();
            UpdateBackButtonVisibility();
            UpdateTitleFromPage();
        }
    }

    private void NavigateToHome()
    {
        PageFrame.Navigate(typeof(HomeShellPage), null, new EntranceNavigationTransitionInfo());
        PageTitleText.Text = "Home";
        BackButton.Visibility = Visibility.Collapsed;

        // Subscribe to home page events
        if (PageFrame.Content is HomeShellPage homePage)
        {
            homePage.SettingsRequested += (s, e) => NavigateToSettings();
        }
    }

    private void NavigateToSettings()
    {
        PageFrame.Navigate(typeof(SettingsPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        PageTitleText.Text = "Settings";
        BackButton.Visibility = Visibility.Visible;

        // Subscribe to settings page events
        if (PageFrame.Content is SettingsPage settingsPage)
        {
            settingsPage.SetDarkThemeState(_themeService.CurrentTheme == ElementTheme.Dark);
            settingsPage.DarkThemeToggled += (s, isDarkTheme) =>
            {
                _themeService.CurrentTheme = isDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
            };
        }
    }

    private void UpdateBackButtonVisibility()
    {
        BackButton.Visibility = PageFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
    }

    private void UpdateTitleFromPage()
    {
        if (PageFrame.Content is SettingsPage)
        {
            PageTitleText.Text = "Settings";
        }
        else if (PageFrame.Content is HomeShellPage)
        {
            PageTitleText.Text = "Home";
        }
    }

    private void OnThemeChanged(object? sender, ElementTheme theme)
    {
        if (PageFrame.Content is SettingsPage settingsPage)
        {
            settingsPage.SetDarkThemeState(theme == ElementTheme.Dark);
        }
    }
}

