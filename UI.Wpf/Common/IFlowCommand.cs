using System;

namespace Palmmedia.BackUp.UI.Wpf.Common
{
    /// <summary>
    /// Interface for flow commands.
    /// </summary>
    public interface IFlowCommand
    {
        /// <summary>
        /// Occurs when an action should be executed.
        /// </summary>
        event Action ExecuteAction;
    }
}
