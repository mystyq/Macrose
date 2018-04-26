using System;
using System.Windows.Input;

namespace Macrose
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _command;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public DelegateCommand(Action<object> command)
            : this(command, null)
        { }

        public DelegateCommand(Action<object> command, Predicate<object> canExecute)
        {
            _command = command ?? throw new NullReferenceException("The command cannot be null.");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _command(parameter);
        }
    }
}
