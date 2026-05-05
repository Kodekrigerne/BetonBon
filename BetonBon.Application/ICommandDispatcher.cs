using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    // Mikkel Klitgaard

    /// <summary>
    /// Defines a contract for asynchronously dispatching command objects to their appropriate handlers.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for locating and invoking the correct
    /// handler for a given command. This abstraction enables decoupling of command senders from their processing logic,
    /// supporting patterns such as CQRS or mediator-based architectures.</remarks>
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command) where TCommand : ICommand<TResponse>;
    }
}
