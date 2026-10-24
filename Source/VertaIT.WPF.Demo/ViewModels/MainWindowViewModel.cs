using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using VertaIT.WPF.Demo.Models;
using VertaIT.WPF.Demo.Views.ObjectPanels;

namespace VertaIT.WPF.Demo.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private ContentControl _selectedObjectPanel;
        private TextFormattingMode _textFormattingMode = TextFormattingMode.Display;

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
            ObjectPanelsCollection.Add(new DatePickerPanel());
            ObjectPanelsCollection.Add(new NumericUpDownPanel());

            if (ObjectPanelsCollection.Count > 0)
            {
                SelectedObjectPanel = ObjectPanelsCollection[0];
            }
        }

        public ObservableCollection<ContentControl> ObjectPanelsCollection { get; } = new ObservableCollection<ContentControl>();

        public ContentControl SelectedObjectPanel
        {
            get => _selectedObjectPanel;
            set => RaisePropertyChanged(ref _selectedObjectPanel, value);
        }

        public TextFormattingMode TextFormattingMode
        {
            get => _textFormattingMode;
            set => RaisePropertyChanged(ref _textFormattingMode, value);
        }
    }
}