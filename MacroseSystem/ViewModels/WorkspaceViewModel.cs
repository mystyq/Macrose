using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Macrose
{
    public abstract class WorkspaceViewModel : ViewModel
    {
        private DelegateCommand _closeCommand;
        private ObservableCollection<CommandViewModel> _commands;

        public EventHandler RequestClose;

        public WorkspaceViewModel(string displayName) : base(displayName)
        {
            CreateCommands();
        }

        public ObservableCollection<CommandViewModel> Commands
        {
            get
            {
                if (_commands == null)
                    _commands = new ObservableCollection<CommandViewModel>();

                return _commands;
            }
        }

        public Action<bool> CloseAction { get; set; }

        public ICommand CloseCommand
        {
            get
            {
                //Lazy instantiate
                if (_closeCommand == null)
                    _closeCommand = new DelegateCommand(c => OnRequestClose());

                return _closeCommand;
            }
        }

        private void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void CreateCommands();
    }
}
