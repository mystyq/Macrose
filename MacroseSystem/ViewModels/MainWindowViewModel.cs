using Macrose.ViewModels;
using Macrose.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace Macrose
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        public MainWindowViewModel()
            : base("Macrose")
        {
            this.ActivateView("Dashboard");
        }

        public ObservableCollection<UserControl> Views
        {
            get;
            private set;
        } = new ObservableCollection<UserControl>();

        private UserControl currentView;
        public UserControl CurrentView
        {
            get
            {
                return this.currentView;
            }
            private set
            {
                if (value != this.currentView)
                {
                    this.currentView = value;
                    OnPropertyChanged("CurrentView");
                }
            }
        }

        protected override void CreateCommands()
        {
            Commands.Add(new CommandViewModel("Dashboard", "dashboard.png", new DelegateCommand(x => this.ActivateView("Dashboard"))));
            Commands.Add(new CommandViewModel("Settings", "settings.png", new DelegateCommand(x => this.ActivateView("Settings"))));
        }

        public void ActivateView(string displayName)
        {
            if (this.CurrentView != null)
            {
                if (this.CurrentView.GetType() == typeof(MacroRecorderView))
                {
                    (this.CurrentView.DataContext as MacroRecorderViewModel).Dispose();
                    this.CurrentView.DataContext = null;
                }
                else if (this.CurrentView.GetType() == typeof(MacroPlaybackView))
                {
                    foreach(var macroView in (this.CurrentView.DataContext as MacroPlaybackViewModel).Macros.ToList())
                    {
                        var macroViewModel = macroView.DataContext as MacroViewModel;
                        macroViewModel.Dispose();
                        macroView.DataContext = null;
                    }
                }
            }
            
            UserControl view = null;
            WorkspaceViewModel viewModel = null;
            switch (displayName)
            {
                case "Dashboard":
                    view = new DashboardView();
                    viewModel = new DashboardViewModel();
                    break;
                case "Settings":
                    view = new SettingsView();
                    viewModel = new SettingsViewModel();
                    break;
                case "MacroRecorder":
                    view = new MacroRecorderView();
                    viewModel = new MacroRecorderViewModel();
                    break;
                case "MacroPlayback":
                    view = new MacroPlaybackView();
                    viewModel = new MacroPlaybackViewModel();
                    break;
            }
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            view.DataContext = viewModel;
            this.CurrentView = view;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string[] message = e.PropertyName.Split('|');
            if (message[0].Equals("ActivateView"))
            {
                this.ActivateView(message[1]);
            }
        }
    }
}
