using System;
using System.Collections.Generic;
using System.Text;
using BetonBon.Application;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
