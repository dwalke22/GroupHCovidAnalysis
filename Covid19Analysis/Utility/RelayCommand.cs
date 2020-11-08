using System;
using System.Windows.Input;

namespace Covid19Analysis.Utility
{
    /// <summary>
    ///     The Relay Command class
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        /// <summary>
        ///     The Relay command constructor
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        ///     Determines if a method can execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            var result = canExecute?.Invoke(parameter) ?? true;
            return result;
        }

        /// <summary>
        ///     Executes the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter)) execute(parameter);
        }

        /// <summary>
        ///     The CanExecuteChanged event handler
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Typically, protected but made public, so can trigger a manual refresh on the result of CanExecute.
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}