using System;
using System.Windows.Input;

namespace Tolarian.Copyshop.ScreenPresenter.Base
{
    public class Command : ICommand
    {
        #region Declaration

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        #endregion

        #region Constructor

        public Command(Action<object> execute)
            : this(execute, null)
        {
        }

        public Command(Action<object> execute, Predicate<object> canExecute)
            : base()
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter)
            => _canExecute == null ? true : _canExecute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}
