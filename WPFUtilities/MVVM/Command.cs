namespace WPFUtilities.MVVM
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    [Serializable]
    public class Command : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public Command(Action<object> execute) : this(execute, null)
        {

        }

        public Command(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            
            return canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
