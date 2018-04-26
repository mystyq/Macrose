using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Macrose
{
    public class CommandViewModel : ViewModel
    {
        public CommandViewModel(
            string displayName, 
            string image, 
            ICommand command,
            bool isDefault = false,
            bool isCancel = false
            ) : base(displayName)
        {
            Command = command;
            IsDefault = isDefault;
            IsCancel = isCancel;
            if (image != null)
            {
                try
                {
                    this.Resource = new BitmapImage(new Uri("pack://application:,,,/Resources/" + image, UriKind.Absolute));
                }
                catch
                {

                }
            }
        }

        public ICommand Command { get; private set; }

        public bool IsDefault { get; private set; }

        public bool IsCancel { get; private set; }

        public object Resource { get; private set; }
    }
}
