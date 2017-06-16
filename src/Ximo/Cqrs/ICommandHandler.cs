namespace Ximo.Cqrs
{
    /// <summary>
    ///     Defines the contract for a command handler as specified in the CQRS pattern.
    /// </summary>
    /// <remarks>
    ///     1. A command is a DTO representing a request for a change in the domain.
    ///     2. A single command usually is mapped to a single command handler.
    ///     3. In CQRS command handlers represent an implementation for Application Services (as defined in domain driven
    ///     design) that cause a change in the domain.
    /// </remarks>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : class
    {
        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <param name="command">The command to be processed.</param>
        void Handle(TCommand command);
    }
}