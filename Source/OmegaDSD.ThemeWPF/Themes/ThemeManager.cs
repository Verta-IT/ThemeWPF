using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OmegaDSD.ThemeWPF.Themes
{
    [Serializable]
    internal class MissingThemeDictionaryRelativePath : Exception
    {
        public MissingThemeDictionaryRelativePath(string theme) : base($"Missing relative theme path attribute for: {theme}")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ThemeDictionaryRelativePath : Attribute
    {
        public string Path { get; private set; }

        public ThemeDictionaryRelativePath(string path)
        {
            Path = path;
        }
    }

    public enum Theme
    {
        [ThemeDictionaryRelativePath("Themes/LightTheme.xaml")]
        Light,
        [ThemeDictionaryRelativePath("Themes/DarkTheme.xaml")]
        Dark
    }

    public static class ThemeManager
    {
        /// <summary>
        /// Get theme relative path saved in the member attribute of the Theme type.
        /// </summary>
        /// <param name="theme">A member of Theme for which the relative path is to be returned</param>
        /// <returns>Theme relative path.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="MissingThemeDictionaryRelativePath"></exception>
        private static string GetThemeRelativePath(Theme theme)
        {
            Type enumType = typeof(Theme);

            MemberInfo[] memberInfos = enumType.GetMember(theme.ToString());

            if (memberInfos.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(theme));
            }

            MemberInfo valueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);

            object[] attribute = valueMemberInfo.GetCustomAttributes(typeof(ThemeDictionaryRelativePath), false);

            if (attribute.Length == 0)
            {
                throw new MissingThemeDictionaryRelativePath(theme.ToString());
            }

            return (attribute[0] as ThemeDictionaryRelativePath).Path;
        }

        /// <summary>
        /// Get all available theme paths based on Theme members
        /// </summary>
        /// <returns>An array that contains all available theme relative paths</returns>
        public static string[] GetAvailableRelativeThemePaths()
        {
            Theme[] themes = (Theme[])Enum.GetValues(typeof(Theme));

            List<string> tmp = new List<string>();

            foreach (Theme theme in themes)
            {
                tmp.Add(GetThemeRelativePath(theme));
            }

            return tmp.ToArray();
        }

        /// <summary>
        /// Chceck if there is a theme resource in the application merged dictionary and return it if so.
        /// </summary>
        /// <returns>The resource dictionary corresponding to the current theme or null value if theme not found.</returns>
        private static ResourceDictionary FindThemeDictionaryInAppMergedDictionaries()
        {
            string[] availableThemePaths = GetAvailableRelativeThemePaths();

            for (int i = 0; i < Application.Current.Resources.MergedDictionaries.Count; i++)
            {
                foreach (string themePath in availableThemePaths)
                {
                    if (Application.Current.Resources.MergedDictionaries[i].Source.OriginalString == themePath)
                    {
                        return Application.Current.Resources.MergedDictionaries[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Chceck if there is a theme resource in the application merged dictionary and convert it to right Theme member.
        /// </summary>
        /// <returns>The Theme member corresponding to the current theme or null value if theme not found.</returns>
        private static Theme? FindThemeInAppMergedDictionaries()
        {
            if (ThemeDictionary != null)
            {
                Theme[] themes = (Theme[])Enum.GetValues(typeof(Theme));

                foreach (Theme theme in themes)
                {
                    if (ThemeDictionary.Source.OriginalString == GetThemeRelativePath(theme))
                    {
                        return theme;
                    }
                }
            }

            return null;
        }

        static ThemeManager()
        {
            currentTheme = FindThemeInAppMergedDictionaries();
        }

        private static Theme? currentTheme = FindThemeInAppMergedDictionaries();

        public static Theme? CurrentTheme
        {
            get => currentTheme;
            private set => currentTheme = value;
        }

        public static ResourceDictionary ThemeDictionary
        {
            get
            {
                return FindThemeDictionaryInAppMergedDictionaries();
            }
            private set
            {
                ResourceDictionary actualThemeDictionary = FindThemeDictionaryInAppMergedDictionaries();

                if (actualThemeDictionary != null)
                {
                    if (actualThemeDictionary != value)
                    {
                        if (value == null)
                        {
                            Application.Current.Resources.MergedDictionaries.Remove(actualThemeDictionary);

                            return;
                        }

                        int actualDictIndex = Application.Current.Resources.MergedDictionaries.IndexOf(actualThemeDictionary);

                        Application.Current.Resources.MergedDictionaries[actualDictIndex] = value;
                    }
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(value);
                }                
            }
        }

        /// <summary>
        /// Change the application theme as specified in the argument
        /// </summary>
        /// <param name="theme">Theme to set</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="MissingThemeDictionaryRelativePath"></exception>
        public static void ChangeTheme(Theme theme)
        {
            if (CurrentTheme != theme)
            {
                try
                {
                    ThemeDictionary = new ResourceDictionary()
                    {
                        Source = new Uri(GetThemeRelativePath(theme), UriKind.Relative)
                    };

                    CurrentTheme = theme;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Remove theme from application merged dictionaries
        /// </summary>
        public static void RemoveTheme()
        {
            ThemeDictionary = null;
            CurrentTheme = null;
        }
    }
}