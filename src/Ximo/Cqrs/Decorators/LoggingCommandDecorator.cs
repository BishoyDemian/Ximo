using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Ximo.Cqrs.Decorators
{
    /// <summary>
    ///     Provides a decorator for command handlers that logs entry, exit and errors resulting from executing a command as
    ///     well as provides execution elapsed time. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <seealso cref="Ximo.Cqrs.Decorators.ICommandDecorator{TCommand}" />
    public sealed class LoggingCommandDecorator<TCommand> : ICommandDecorator<TCommand>
        where TCommand : class
    {
        private readonly ICommandHandler<TCommand> _decorated;
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoggingCommandDecorator{TCommand}" /> class.
        /// </summary>
        /// <param name="decorated">The decorated command handler.</param>
        /// <param name="logger">The configured logger.</param>
        public LoggingCommandDecorator(ICommandHandler<TCommand> decorated,
            ILogger<LoggingCommandDecorator<TCommand>> logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        /// <summary>
        ///     Handles the specified command whiel applying decorator logic.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        public void Handle(TCommand command)
        {
            var commandName = command.GetType().Name;

            _logger.LogInformation($"Start executing command '{commandName}'" + Environment.NewLine);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                _decorated.Handle(command);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception throw while executing command '{commandName}'" +
                                 Environment.NewLine + e.Message + Environment.NewLine);
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.LogInformation($"Executed command '{commandName}' in {stopwatch.Elapsed}." +
                                   Environment.NewLine);
        }
    }
}