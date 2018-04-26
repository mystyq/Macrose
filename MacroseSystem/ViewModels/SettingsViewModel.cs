using System.ComponentModel;

namespace Macrose.ViewModels
{
    public class SettingsViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        public SettingsViewModel() 
            : base(string.Empty)
        {

        }

        protected override void CreateCommands()
        {
            
        }
    }
}
