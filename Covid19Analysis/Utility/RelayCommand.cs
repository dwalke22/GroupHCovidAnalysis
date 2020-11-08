using System;
using System.Windows.Input;

namespace Covid19Analysis.Utility
{
    /// <summary>
    ///     The Relay Command class
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Data members

        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        #endregion

        #region Constructors

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

        #endregion

        #region Methods

        /// <summary>
        ///     Determines if a method can execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            var result = this.canExecute?.Invoke(parameter) ?? true;
            return result;
        }

        /// <summary>
        ///     Executes the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.execute(parameter);
            }
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
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}