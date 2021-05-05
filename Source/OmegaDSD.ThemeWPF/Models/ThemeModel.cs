using OmegaDSD.ThemeWPF.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaDSD.ThemeWPF.Models
{
    public class ThemeModel : NotifyPropertyChanged
    {
        public ThemeModel(Theme? theme)
        {
            Theme = theme;
            Name = (theme is null) ? "None" : theme.ToString();
        }

        public string Name { get; private set; }

        public Theme? Theme { get; private set; }

        public bool IsSelected
        {
            get => ThemeManager.CurrentTheme == Theme;
            set
            {
                if (ThemeManager.CurrentTheme != Theme)
                {
                    if (Theme is null)
                    {
                        ThemeManager.RemoveTheme();
                    }
                    else
                    {
                        ThemeManager.ChangeTheme((Theme)Theme);
                    }

                    RaisePropertyChanged();
                }
            }
        }
    }
}
