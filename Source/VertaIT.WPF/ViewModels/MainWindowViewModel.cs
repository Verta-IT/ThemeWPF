using VertaIT.WPF.Models;
using VertaIT.WPF.Views.ObjectPanels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Controls;
using System.Windows.Media;

namespace VertaIT.WPF.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            ObjectPanelsCollection.Add(new DemoPanel());
            ObjectPanelsCollection.Add(new BasicControlsPanel());
            ObjectPanelsCollection.Add(new SlidersPanel());
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
        }

        private ContentControl selectedObjectPanel;       

        public ObservableCollection<ContentControl> ObjectPanelsCollection { get; } = new ObservableCollection<ContentControl>();        

        public ContentControl SelectedObjectPanel
        {
            get => selectedObjectPanel;
            set => RaisePropertyChanged(ref selectedObjectPanel, value);
        }

        private TextFormattingMode textFormattingMode = TextFormattingMode.Display;

        public TextFormattingMode TextFormattingMode
        {
            get => textFormattingMode;
            set => RaisePropertyChanged(ref textFormattingMode, value);
        }
    }
}
