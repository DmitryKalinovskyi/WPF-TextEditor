using System.Windows.Input;

namespace Word.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object?> _canExecute;
        private readonly Action<object?> _execute;

        public RelayCommand(Action<object?> execute)
        {
            _canExecute = (param) => true;
            _execute = execute;
        }

        public RelayCommand(Predicate<object?> canExecute, Action<object?> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
