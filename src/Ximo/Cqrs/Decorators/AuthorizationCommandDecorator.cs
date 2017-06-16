using Ximo.Cqrs.Security;

namespace Ximo.Cqrs.Decorators
{
    /// <summary>
    ///     Provides a decorator for authorizing commands. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <seealso cref="Ximo.Cqrs.Decorators.ICommandDecorator{TCommand}" />
    public sealed class AuthorizationCommandDecorator<TCommand> : ICommandDecorator<TCommand>
        where TCommand : class
    {
        private readonly IAuthorizationManager _authorizationManager;
        private readonly ICommandHandler<TCommand> _decorated;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizationCommandDecorator{TCommand}" /> class.
        /// </summary>
        /// <param name="decorated">The decorated command handler.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public AuthorizationCommandDecorator(ICommandHandler<TCommand> decorated,
            IAuthorizationManager authorizationManager)
        {
            _decorated = decorated;
            _authorizationManager = authorizationManager;
        }

        /// <summary>
        ///     Runs the authorization rules for the specified commands.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Handle(TCommand command)
        {
            _authorizationManager.Authorize(command);
            _decorated.Handle(command);
        }
    }
}