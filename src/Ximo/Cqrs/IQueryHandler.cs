namespace Ximo.Cqrs
{
    /// <summary>
    ///     Defines the contract for a query router.
    /// </summary>
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : class
    {
        /// <summary>
        ///     Processes the query and returns back the relevant response.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the query response.</typeparam>
        /// <param name="query">The query to be processed.</param>
        /// <returns>The relevant query response.</returns>
        TResult Read(TQuery query);
    }
}