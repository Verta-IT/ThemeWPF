using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using VertaIT.WPF.Theme;

namespace VertaIT.WPF.Tests
{
    [TestClass]
    public class ThemeManagerTests
    {
        private const string LightThemeUriPack = @"pack://application:,,,/VertaIT.WPF;component/Themes/LightTheme.xaml";
        private const string DarkThemeUriPack = @"pack://application:,,,/VertaIT.WPF;component/Themes/DarkTheme.xaml";
        private const string MissingThemeUriPack = @"pack://application:,,,/VertaIT.WPF;component/Themes/InvalidThemeFile.xaml";
        private const string InvalidThemeUriPack = @"pa!ck:/!/appli#cation:,,,/Ver!taIT.WPF;component/Themes/InvalidThemeFile.xaml";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "This is neccessary to parse a WPF URI. The 'pack' uri schema is added when a System.Windows.Application is created")]
        private static readonly Application _application = new Application();

        public enum CorrectTheme
        {
            [ResourceUriPath(LightThemeUriPack)]
            Light,
            [ResourceUriPath(DarkThemeUriPack)]
            Dark
        }

        public enum IncorrectTheme
        {
            [ResourceUriPath(MissingThemeUriPack)]
            Missing,
            [ResourceUriPath(InvalidThemeUriPack)]
            Invalid
        }

        public enum IncorrectAndCorrectTheme
        {
            [ResourceUriPath(LightThemeUriPack)]
            Light,
            [ResourceUriPath(MissingThemeUriPack)]
            Missing,
            [ResourceUriPath(InvalidThemeUriPack)]
            Invalid,
            [ResourceUriPath(DarkThemeUriPack)]
            Dark
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ShouldThrowArgumentNullException()
        {
            // Act
            _ = new ThemeManager<CorrectTheme>(null);
        }

        [TestMethod]
        public void Ctor_ShouldSetThemeSource()
        {
            // Arrange
            var themeSource = new Collection<ResourceDictionary>();

            // Act
            var themeManager = new ThemeManager<CorrectTheme>(themeSource);

            // Assert
            Assert.AreEqual(themeSource, themeManager.ThemeSource);
        }

        [TestMethod]
        [DataRow(LightThemeUriPack, CorrectTheme.Light)]
        [DataRow(DarkThemeUriPack, CorrectTheme.Dark)]
        public void GetActualTheme_ShouldGetActualTheme(string themeUriPath, CorrectTheme theme)
        {
            // Arrange
            var themeManager = CreateMockedThemeManager(themeUriPath);

            // Act
            var actual = themeManager.GetActualTheme();

            // Assert
            Assert.AreEqual(actual, theme);
        }

        [TestMethod]
        public void GetActualTheme_ShouldGetNull()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<CorrectTheme>();

            // Act
            var actual = themeManager.GetActualTheme();

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        [DataRow(LightThemeUriPack, CorrectTheme.Light)]
        [DataRow(DarkThemeUriPack, CorrectTheme.Dark)]
        public void ChangeTheme_ShouldSetThemeIfThereIsNoOneInTheSource(string themeUriPath, CorrectTheme theme)
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<CorrectTheme>();

            // Act
            themeManager.ChangeTheme(theme);

            // Assert
            Assert.AreEqual(themeManager.GetActualTheme(), theme);
            Assert.AreEqual(themeManager.ThemeSource.Count, 1);
            Assert.IsTrue(themeManager.ThemeSource.Any(item => item.Source.OriginalString == themeUriPath));
        }

        [TestMethod]
        [DataRow(LightThemeUriPack, CorrectTheme.Dark, DarkThemeUriPack)]
        [DataRow(DarkThemeUriPack, CorrectTheme.Light, LightThemeUriPack)]
        public void ChangeTheme_ShouldChangeTheme(string baseThemeUriPath, CorrectTheme themeToChange, string changedThemeUriPath)
        {
            // Arrange
            var themeManager = CreateMockedThemeManager(baseThemeUriPath);

            // Act
            themeManager.ChangeTheme(themeToChange);

            // Assert
            Assert.AreEqual(themeManager.GetActualTheme(), themeToChange);
            Assert.AreEqual(themeManager.ThemeSource.Count, 1);
            Assert.IsTrue(themeManager.ThemeSource.Any(item => item.Source.OriginalString == changedThemeUriPath));
        }

        [TestMethod]
        [DataRow(LightThemeUriPack, CorrectTheme.Light)]
        [DataRow(DarkThemeUriPack, CorrectTheme.Dark)]
        public void ChangeTheme_ShouldDoNothing(string themeUriPath, CorrectTheme theme)
        {
            // Arrange
            var themeManager = CreateMockedThemeManager(themeUriPath);

            // Act
            themeManager.ChangeTheme(theme);

            // Assert
            Assert.AreEqual(themeManager.GetActualTheme(), theme);
            Assert.AreEqual(themeManager.ThemeSource.Count, 1);
            Assert.IsTrue(themeManager.ThemeSource.Any(item => item.Source.OriginalString == themeUriPath));
        }

        [TestMethod]
        [ExpectedException(typeof(ChangeThemeException))]
        public void ChangeTheme_MissingResourceAtUriPath_ShouldThrowException()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<IncorrectTheme>();

            // Act
            themeManager.ChangeTheme(IncorrectTheme.Missing);

            // Assert
            Assert.IsNull(themeManager.GetActualTheme());
            Assert.AreEqual(themeManager.ThemeSource.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ChangeThemeException))]
        public void ChangeTheme_InvalidUriPath_ShouldThrowException()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<IncorrectTheme>();

            // Act
            themeManager.ChangeTheme(IncorrectTheme.Invalid);

            // Assert
            Assert.IsNull(themeManager.GetActualTheme());
            Assert.AreEqual(themeManager.ThemeSource.Count, 0);
        }

        [TestMethod]
        [DataRow(LightThemeUriPack)]
        [DataRow(DarkThemeUriPack)]
        public void RemoveTheme_ShouldRemoveTheme(string themeUriPath)
        {
            // Arrange
            var themeManager = CreateMockedThemeManager(themeUriPath);

            // Act
            themeManager.RemoveTheme();

            // Assert
            Assert.IsNull(themeManager.GetActualTheme());
            Assert.AreEqual(themeManager.ThemeSource.Count, 0);
        }

        [TestMethod]
        public void RemoveTheme_ShouldDoNothingIfThereIsNoTheme()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<CorrectTheme>();

            // Act
            themeManager.RemoveTheme();

            // Assert
            Assert.IsNull(themeManager.GetActualTheme());
            Assert.AreEqual(themeManager.ThemeSource.Count, 0);
        }

        [TestMethod]
        [DataRow(LightThemeUriPack, CorrectTheme.Light)]
        [DataRow(DarkThemeUriPack, CorrectTheme.Dark)]
        public void ChangeTheme_ShouldSetThemeAfterThemeRemoved(string themeUriPath, CorrectTheme theme)
        {
            // Arrange
            var themeManager = CreateMockedThemeManager(themeUriPath);

            // Act
            themeManager.RemoveTheme();
            themeManager.ChangeTheme(theme);

            // Assert
            Assert.AreEqual(themeManager.GetActualTheme(), theme);
            Assert.AreEqual(themeManager.ThemeSource.Count, 1);
            Assert.IsTrue(themeManager.ThemeSource.Any(item => item.Source.OriginalString == themeUriPath));
        }

        [TestMethod]
        public void GetAvailableThemes_ShouldGetAllCorrectThemes()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<CorrectTheme>();

            var themeDictionary = CreateCountEnumThemeDictionary<CorrectTheme>();

            // Act
            var returnedThemes = themeManager.GetAvailableThemes();

            CountThemes(themeDictionary, returnedThemes);

            // Assert
            Assert.IsTrue(themeDictionary.All(item => item.Value == 1));
        }

        [TestMethod]
        public void GetAvailableThemes_ShouldGetAllCorrectThemesButAvoidIncorrect()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<IncorrectAndCorrectTheme>();

            var themeDictionary = CreateCountEnumThemeDictionary<IncorrectAndCorrectTheme>();

            // Act
            var returnedThemes = themeManager.GetAvailableThemes();

            CountThemes(themeDictionary, returnedThemes);

            // Assert
            Assert.AreEqual(themeDictionary.Count, 4, "This check is only for unit tests because in this case developer has to modify asserts if the themes (Enum) has been modified");
            Assert.AreEqual(themeDictionary[IncorrectAndCorrectTheme.Dark], 1);
            Assert.AreEqual(themeDictionary[IncorrectAndCorrectTheme.Light], 1);
            Assert.AreEqual(themeDictionary[IncorrectAndCorrectTheme.Missing], 0);
            Assert.AreEqual(themeDictionary[IncorrectAndCorrectTheme.Invalid], 0);
        }

        [TestMethod]
        public void GetAvailableThemes_ShouldGetNone()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<IncorrectTheme>();

            // Act
            var returnedThemes = themeManager.GetAvailableThemes();

            // Assert
            Assert.AreEqual(returnedThemes.Count(), 0);
        }

        [TestMethod]
        public void GetIncorrectThemes_ShouldGetIncorrectThemesWithMessage()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<IncorrectTheme>();

            var themeDictionary = CreateCountEnumThemeDictionary<IncorrectTheme>();

            // Act
            var returnedThemes = themeManager.GetIncorrectThemes();

            CountIncorrectThemes(themeDictionary, returnedThemes);

            // Assert
            Assert.IsTrue(themeDictionary.All(item => item.Value == 1));
            Assert.IsTrue(returnedThemes.All(item => !string.IsNullOrEmpty(item.Value)));
        }

        [TestMethod]
        public void GetIncorrectThemes_ShouldGetNone()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<CorrectTheme>();

            // Act
            var returnedThemes = themeManager.GetIncorrectThemes();

            // Assert
            Assert.AreEqual(returnedThemes.Count, 0);
        }

        [TestMethod]
        public void GetIncorrectThemes_ShouldGetAllIncorrectThemesButAvoidCorrect()
        {
            // Arrange
            var themeManager = CreateMockedThemeManagerWithEmptySource<IncorrectAndCorrectTheme>();

            var themeDictionary = CreateCountEnumThemeDictionary<IncorrectAndCorrectTheme>();

            // Act
            var returnedThemes = themeManager.GetIncorrectThemes();

            CountIncorrectThemes(themeDictionary, returnedThemes);

            // Assert
            Assert.AreEqual(themeDictionary.Count, 4, "This check is only for unit tests because in this case developer has to modify asserts if the themes (Enum) has been modified");
            Assert.AreEqual(themeDictionary[IncorrectAndCorrectTheme.Dark], 0);
            Assert.AreEqual(themeDictionary[IncorrectAndCorrectTheme.Light], 0);
            Assert.AreEqual(themeDictionary[IncorrectAndCorrectTheme.Missing], 1);
            Assert.AreEqual(themeDictionary[IncorrectAndCorrectTheme.Invalid], 1);
        }

        private ThemeManager<CorrectTheme> CreateMockedThemeManager(string themeUriPath = LightThemeUriPack)
        {
            return new ThemeManager<CorrectTheme>(new Collection<ResourceDictionary>
            {
                new ResourceDictionary
                {
                    Source = new Uri(themeUriPath, UriKind.Absolute)
                }
            });
        }

        private ThemeManager<TTheme> CreateMockedThemeManagerWithEmptySource<TTheme>() where TTheme : struct, Enum
        {
            return new ThemeManager<TTheme>(new Collection<ResourceDictionary>());
        }

        private Dictionary<TTheme, int> CreateCountEnumThemeDictionary<TTheme>()
        {
            var themes = (TTheme[])Enum.GetValues(typeof(TTheme));

            var result = new Dictionary<TTheme, int>();

            foreach (var theme in themes)
            {
                result.Add(theme, 0);
            }

            return result;
        }

        private static void CountThemes<TTheme>(Dictionary<TTheme, int> themeDictionary, IEnumerable<TTheme> returnedThemes)
        {
            foreach (var returnedTheme in returnedThemes)
            {
                if (themeDictionary.ContainsKey(returnedTheme))
                {
                    themeDictionary[returnedTheme]++;
                }
            }
        }

        private static void CountIncorrectThemes<TTheme>(Dictionary<TTheme, int> themeDictionary, Dictionary<TTheme, string> returnedThemes)
        {
            foreach (var returnedTheme in returnedThemes)
            {
                if (themeDictionary.ContainsKey(returnedTheme.Key))
                {
                    themeDictionary[returnedTheme.Key]++;
                }
            }
        }
    }
}
