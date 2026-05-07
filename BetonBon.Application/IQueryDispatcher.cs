using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    /// <summary>
    /// Defines a contract for dispatching query objects to their corresponding handlers and asynchronously returning
    /// the result.
    /// </summary>
    /// <remarks>Implementations of this interface are typically used in applications following the CQRS
    /// pattern to decouple query handling logic from the caller. Thread safety and handler registration 
    /// behavior depend on the specific implementation.</remarks>
   
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
