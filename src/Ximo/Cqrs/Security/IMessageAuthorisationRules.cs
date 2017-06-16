using System.Collections.Generic;

namespace Ximo.Cqrs.Security
{
    /// <summary>
    ///     Defines a contract for collection of authorisan rules to apply to a message.
    /// </summary>
    /// <typeparam name="TMessage">The type of the t message.</typeparam>
    public interface IMessageAuthorisationRules<in TMessage> where TMessage : class
    {
        /// <summary>
        ///     Gets the authorisation rules to authorise an operation.
        /// </summary>
        IEnumerable<IAuthorizationRule<TMessage>> Rules { get; }
    }
}