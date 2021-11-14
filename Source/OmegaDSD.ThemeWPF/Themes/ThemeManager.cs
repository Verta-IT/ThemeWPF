using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VertaIT.ThemeWPF.Themes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ResourceUriPathAttribute : Attribute
    {
        public ResourceUriPathAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }

    public class ThemeManager<T> where T : struct, Enum
    {
        public ThemeManager() : this(Application.Current.Resources.MergedDictionaries)
        {
        }

        public ThemeManager(Collection<ResourceDictionary> mergedDictionaries)
        {
            ThemeSource = mergedDictionaries ?? throw new ArgumentNullException(nameof(mergedDictionaries), "Source of the themes cannot be null");
        }

        public T? CurrentTheme { get; private set; }

        public Collection<ResourceDictionary> ThemeSource { get; }

        private static ResourceDictionary CreateResourceDictionary(string uriStr)
        {
            return new ResourceDictionary()
            {
                Source = new Uri(uriStr, UriKind.RelativeOrAbsolute)
            };
        }

        private static bool IsResourceDictionaryExists(string uriStr)
        {
            if (string.IsNullOrEmpty(uriStr))
            {
                return false;
            }

            try
            {
                _ = CreateResourceDictionary(uriStr);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GetUriPath(T theme)
        {
            return typeof(T)
                .GetMember(theme.ToString())
                .FirstOrDefault(m => m.DeclaringType == typeof(T))
                ?.GetCustomAttributes(typeof(ResourceUriPathAttribute), false)
                .Select(item => ((ResourceUriPathAttribute)item).Path)
                .FirstOrDefault();
        }

        private static IEnumerable<(T Theme, string Uri)> GetThemesData()
        {
            var themes = (T[])Enum.GetValues(typeof(T));

            return themes
                .Select(theme => (theme, GetUriPath(theme)))
                .ToList();
        }

        private static IEnumerable<(T Theme, string Uri)> GetValidThemesData()
        {
            return GetThemesData()
                .Where(item => IsResourceDictionaryExists(item.Uri))
                .Select(item => (item.Theme, item.Uri))
                .ToList();
        }

        private static IEnumerable<(T Theme, string Uri)> GetThemesThatDoNotExist()
        {
            return GetThemesData()
                .Where(item => !IsResourceDictionaryExists(item.Uri))
                .Select(item => (item.Theme, item.Uri))
                .ToList();
        }

        private (T Theme, int Index, string Uri)? GetActualThemeData()
        {
            if (ThemeSource != null)
            {
                var themesData = GetValidThemesData();

                for (int i = 0; i < ThemeSource.Count; i++)
                {
                    foreach (var themeData in themesData)
                    {
                        if (ThemeSource[i].Source.OriginalString == themeData.Uri)
                        {
                            return (themeData.Theme, i, themeData.Uri);
                        }
                    }
                }
            }

            return null;
        }

        public void ChangeTheme(T theme)
        {
            var actual = GetActualThemeData();

            var uriStr = GetUriPath(theme);

            var newTheme = CreateResourceDictionary(uriStr);

            if (actual.HasValue)
            {
                ThemeSource[actual.Value.Index] = newTheme;
            }
            else
            {
                ThemeSource.Add(newTheme);
            }
        }

        public void RemoveTheme()
        {
            var actual = GetActualThemeData();

            if (actual.HasValue)
            {
                ThemeSource.RemoveAt(actual.Value.Index);
            }
        }

        public T? GetActualTheme()
        {
            var actual = GetActualThemeData();

            return actual.HasValue ? actual.Value.Theme : (T?)null;
        }

        public IEnumerable<T> GetAvailableThemes()
        {
            return GetValidThemesData()
                .Select(data => data.Theme)
                .ToList();
        }

        public Dictionary<T, string> GetIncorrectThemes()
        {
            var themes = GetThemesThatDoNotExist();

            var result = new Dictionary<T, string>();

            foreach (var theme in themes)
            {
                result.Add(theme.Theme, $"The resource at '{theme.Uri}' of the theme '{theme.Theme}' doesn't exist");
            }

            return result;
        }
    }
}