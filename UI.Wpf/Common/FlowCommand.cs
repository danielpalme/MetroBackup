using System;
using System.Windows.Input;

namespace Palmmedia.BackUp.UI.Wpf.Common
{
    /// <summary>
    /// <see cref="ICommand"/> implementation that throws an event when it should be executed.
    /// </summary>
    public class FlowCommand : ICommand, IFlowCommand
    {
        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowCommand"/> class.
        /// </summary>
        public FlowCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowCommand"/> class.
        /// </summary>
        /// <param name="canExecute">Determines whether the command can be executed.</param>
        public FlowCommand(Predicate<object> canExecute)
        {
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Occurs when command is executed.
        /// </summary>
        public event Action ExecuteAction = delegate { };

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            this.ExecuteAction();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute(parameter);
        }
    }
}
