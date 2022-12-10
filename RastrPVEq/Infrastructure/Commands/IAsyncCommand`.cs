using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RastrPVEq.Infrastructure.Commands
{
    /// <summary>
    /// IAsyncCommand interface
    /// </summary>
    public interface IAsyncCommand<in T> : ICommand
    {
        /// <summary>
        /// Gets running tasks
        /// </summary>
        IEnumerable<Task> RunningTasks { get; }

        /// <summary>
        /// Can execute method
        /// </summary>
        /// <returns></returns>
        bool CanExecute(T parameter);

        /// <summary>
        /// Execute async method
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync(T parameter);
    }
}
