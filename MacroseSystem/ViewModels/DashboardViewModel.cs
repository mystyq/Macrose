using System.ComponentModel;

namespace Macrose.ViewModels
{
    public class DashboardViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        public DashboardViewModel()
            : base(string.Empty)
        {
            this.ViewMacroRecorder = new DelegateCommand(x => OnPropertyChanged("ActivateView|MacroRecorder"));
            this.ViewMacroPlayback = new DelegateCommand(x => OnPropertyChanged("ActivateView|MacroPlayback"));
        }

        public DelegateCommand ViewMacroRecorder
        {
            get; private set;
        }

        public DelegateCommand ViewMacroPlayback
        {
            get; private set;
        }

        protected override void CreateCommands()
        {
        }
    }
}
