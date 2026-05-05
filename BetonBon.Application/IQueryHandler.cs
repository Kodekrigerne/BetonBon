using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{

    /// <summary>
    /// Defines a handler for processing a query and returning a result asynchronously.
    /// </summary>
    /// <remarks>Implementations should ensure thread safety if the handler is intended to be used
    /// concurrently.</remarks>
    /// <typeparam name="TQuery">The type of the query to handle. Must implement <see cref="IQuery{TResult}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the handler.</typeparam>
    
    public interface IQueryHandler<in TQuery, TResult> 
        where TQuery : IQuery<TResult>
    {
        Task<TResult?> HandleAsync(TQuery query);
    }
}
