using System;
using System.Collections.Generic;
using System.Text;
using BetonBon.Application;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BetonBon.Infrastructure
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        async Task ICommandDispatcher.DispatchAsync<TCommand>(TCommand command)
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command);
        }

        async Task<TResponse> ICommandDispatcher.DispatchAsync<TCommand, TResponse>(TCommand command)
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
            var result = await handler.HandleAsync(command);

            return result!;
        }
    }
}
