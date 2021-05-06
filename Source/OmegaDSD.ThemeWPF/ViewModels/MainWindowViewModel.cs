using OmegaDSD.ThemeWPF.Models;
using OmegaDSD.ThemeWPF.Themes;
using OmegaDSD.ThemeWPF.Views.ObjectPanels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Controls;

namespace OmegaDSD.ThemeWPF.ViewModels
{
    public class ThemeCollection : ObservableCollection<ThemeModel>
    {
        public ThemeCollection()
        {
            Array themeList = Enum.GetValues(typeof(Theme));

            Add(new ThemeModel(null));

            foreach (object theme in themeList)
            {
                Add(new ThemeModel((Theme)theme));
            }
        }
    }

    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            ObjectPanelsCollection.Add(new DemoPanel());
            ObjectPanelsCollection.Add(new CheckBoxesPanel());
            ObjectPanelsCollection.Add(new TextsPanel());
            ObjectPanelsCollection.Add(new TextBoxesPanel());
            ObjectPanelsCollection.Add(new SlidersPanel());
            ObjectPanelsCollection.Add(new ButtonsPanel());
            ObjectPanelsCollection.Add(new ListsPanel());
            ObjectPanelsCollection.Add(new ExpandersPanel());
            ObjectPanelsCollection.Add(new TabControlsPanel());
            ObjectPanelsCollection.Add(new TreeViewPanel());
            ObjectPanelsCollection.Add(new MenuPanel());
            ObjectPanelsCollection.Add(new CalendarPanel());

            if (ObjectPanelsCollection.Count > 0)
            {
                SelectedObjectPanel = ObjectPanelsCollection[0];
            }

            foreach (ThemeModel themeModel in ThemeCollection)
            {
                if (themeModel.Theme == ThemeManager.CurrentTheme)
                {
                    selectedTheme = themeModel;
                }
            }
        }

        private ThemeModel selectedTheme;
        private ContentControl selectedObjectPanel;       

        public ThemeCollection ThemeCollection { get; } = new ThemeCollection();

        public ThemeModel SelectedTheme
        {
            get => selectedTheme;
            set
            {
                if (value != selectedTheme)
                {
                    if (value.Theme is null)
                    {
                        ThemeManager.RemoveTheme();
                    }
                    else
                    {
                        ThemeManager.ChangeTheme((Theme)value.Theme);
                    }

                    RaisePropertyChanged(ref selectedTheme, value);
                }                
            }
        }

        public ObservableCollection<ContentControl> ObjectPanelsCollection { get; } = new ObservableCollection<ContentControl>();        

        public ContentControl SelectedObjectPanel
        {
            get => selectedObjectPanel;
            set => RaisePropertyChanged(ref selectedObjectPanel, value);
        }
    }
}
