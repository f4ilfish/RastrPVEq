using System;
using System.Windows.Input;

namespace RastrPVEq.Infrastructure.Commands
{
    /// <summary>
    /// Relay command class
    /// </summary>
    internal class RelayCommand : ICommand
    {
        /// <summary>
        /// Object delegate (action) field
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// Object to bool func field
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// CanExecuteChanged event
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// RelayCommand constructor
        /// </summary>
        /// <param name="Execute"></param>
        /// <param name="CanExecute"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _canExecute = CanExecute;
        }

        /// <summary>
        /// CanExecute command method
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Execute command method
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) => _execute(parameter);
    }
}
