using System;
using System.IO;
using Microsoft.UI.Xaml;

namespace FluentGroupsServices
{
    /// <summary>
    /// Service for managing app theme (light/dark mode).
    /// </summary>
    public class ThemeService
    {
        private readonly string _themeFilePath;
        private ElementTheme _currentTheme = ElementTheme.Dark;
        private FrameworkElement? _themeRoot;

        public event EventHandler<ElementTheme>? ThemeChanged;

        public ElementTheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    PersistTheme(value);
                    ApplyTheme(value);
                    ThemeChanged?.Invoke(this, value);
                }
            }
        }

        public ThemeService()
        {
            var baseFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "FluentGroups");

            Directory.CreateDirectory(baseFolder);
            _themeFilePath = Path.Combine(baseFolder, "theme.config");
            
            // Load saved theme on initialization
            _currentTheme = GetSavedTheme();
        }

        /// <summary>
        /// Get saved theme from storage
        /// </summary>
        private ElementTheme GetSavedTheme()
        {
            var themeName = "Dark";

            try
            {
                if (File.Exists(_themeFilePath))
                {
                    themeName = File.ReadAllText(_themeFilePath).Trim();
                }
            }
            catch
            {
            }

            if (Enum.TryParse<ElementTheme>(themeName, out var parsedTheme)
                && (parsedTheme == ElementTheme.Dark || parsedTheme == ElementTheme.Light || parsedTheme == ElementTheme.Default))
            {
                return parsedTheme;
            }

            return ElementTheme.Dark;
        }

        /// <summary>
        /// Set app theme and persist to storage
        /// </summary>
        public void SetTheme(ElementTheme theme)
        {
            CurrentTheme = theme;
        }

        /// <summary>
        /// Toggle between Light and Dark theme
        /// </summary>
        public void ToggleTheme()
        {
            var newTheme = _currentTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
            CurrentTheme = newTheme;
        }

        /// <summary>
        /// Attach the root visual element that should receive RequestedTheme updates.
        /// </summary>
        public void AttachThemeRoot(FrameworkElement root)
        {
            _themeRoot = root;
            ApplyTheme(_currentTheme);
        }

        /// <summary>
        /// Apply theme to the attached root element.
        /// </summary>
        public void ApplyTheme(ElementTheme theme)
        {
            if (_themeRoot == null)
            {
                return;
            }

            _themeRoot.RequestedTheme = theme;
        }

        private void PersistTheme(ElementTheme theme)
        {
            try
            {
                File.WriteAllText(_themeFilePath, theme.ToString());
            }
            catch
            {
            }
        }

        /// <summary>
        /// Initialize theme on app startup
        /// </summary>
        public void Initialize()
        {
            var theme = GetSavedTheme();
            _currentTheme = theme;
        }
    }
}
