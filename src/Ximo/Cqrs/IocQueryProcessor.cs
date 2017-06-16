using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Validation;

namespace Ximo.Cqrs
{
    /// <summary>
    ///     Default implementation for the query processor using dependency injection for routing queries.
    /// </summary>
    /// <seealso cref="Ximo.Cqrs.IQueryProcessor" />
    internal class IocQueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IocQueryProcessor" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public IocQueryProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        ///     Processes the query.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>The relevant query response.</returns>
        public TResult ProcessQuery<TQuery, TResult>(TQuery query) where TQuery : class
        {
            Check.NotNull(query, nameof(query));
            return _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>().Read(query);
        }

        /// <summary>
        ///     Processes the query asynchronously.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>The relevant query response.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TResult> ProcessQueryAsync<TQuery, TResult>(TQuery query) where TQuery : class
        {
            return await Task<TResult>.Factory.StartNew(() => ProcessQuery<TQuery, TResult>(query));
        }
    }
}