using OmegaDSD.ThemeWPF.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaDSD.ThemeWPF.Models
{
    public class ThemeModel
    {
        public ThemeModel(Theme? theme)
        {
            Theme = theme;
        }

        public Theme? Theme { get; private set; }

        public override string ToString()
        {
            return (Theme is null) ? "None" : Theme.ToString();
        }
    }
}
