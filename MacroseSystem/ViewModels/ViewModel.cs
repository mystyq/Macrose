using System.ComponentModel;

namespace Macrose
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(string displayName)
        {
            this.DisplayName = displayName;
        }

        public string DisplayName { get; private set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
