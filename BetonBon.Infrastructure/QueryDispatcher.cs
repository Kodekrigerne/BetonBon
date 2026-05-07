using System;
using System.Collections.Generic;
using System.Text;
using BetonBon.Application;
using Microsoft.Extensions.DependencyInjection;

namespace BetonBon.Infrastructure
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        async Task<TResult> IQueryDispatcher.DispatchAsync<TQuery, TResult>(TQuery query)
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

            var result= await handler.HandleAsync(query);

            return result!;
        }
    }
}
