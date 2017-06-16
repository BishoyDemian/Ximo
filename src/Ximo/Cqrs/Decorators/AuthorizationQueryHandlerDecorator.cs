using Ximo.Cqrs.Security;

namespace Ximo.Cqrs.Decorators
{
    /// <summary>
    ///     Provides a decorator for authorizing queries. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="Ximo.Cqrs.Decorators.IQueryDecorator{TQuery, TResult}" />
    public sealed class AuthorizationQueryHandlerDecorator<TQuery, TResult> : IQueryDecorator<TQuery, TResult>
        where TQuery : class
    {
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IQueryHandler<TQuery, TResult> _decorated;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizationQueryHandlerDecorator{TQuery, TResult}" /> class.
        /// </summary>
        /// <param name="decorated">The decorated query handler.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public AuthorizationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated,
            IAuthorizationManager authorizationManager)
        {
            _decorated = decorated;
            _authorizationManager = authorizationManager;
        }

        /// <summary>
        ///     Runs the authorization rules for the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>TResult.</returns>
        public TResult Read(TQuery query)
        {
            _authorizationManager.Authorize(query);
            return _decorated.Read(query);
        }
    }
}