using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
