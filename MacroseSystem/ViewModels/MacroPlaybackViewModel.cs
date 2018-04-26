using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Macrose.Views;

namespace Macrose.ViewModels
{
    public class MacroPlaybackViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        private DelegateCommand _addMacroCommand;

        public MacroPlaybackViewModel()
            : base(string.Empty)
        {
            this.AddMacro();
        }

        public DelegateCommand AddMacroCommand
        {
            get
            {
                if (_addMacroCommand == null)
                    _addMacroCommand = new DelegateCommand(c => AddMacro());

                return _addMacroCommand;
            }
        }

        public ObservableCollection<MacroView> Macros
        {
            get;
        } = new ObservableCollection<MacroView>();

        private void MacroViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Delete"))
            {
                this.Macros.Remove(this.Macros.FirstOrDefault(x => x.DataContext == (sender as MacroViewModel)));
                if (this.Macros.Count == 0) this.AddMacro();
            }
        }

        private void AddMacro()
        {
            var macroView = new MacroView();
            var macroViewModel = new MacroViewModel();
            macroViewModel.PropertyChanged += MacroViewModel_PropertyChanged;
            macroView.DataContext = macroViewModel;
            Macros.Add(macroView);
            OnPropertyChanged("Macros");
        }

        protected override void CreateCommands()
        {
        }
    }
}
