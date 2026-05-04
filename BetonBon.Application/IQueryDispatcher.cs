using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
