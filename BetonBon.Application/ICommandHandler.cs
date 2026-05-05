using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    // Mikkel Klitgaard

    /// <summary>
    /// Defines a contract for handling a command asynchronously.
    /// </summary>
    /// <remarks>Implement this interface to provide custom logic for processing specific command types.
    /// Command handlers are typically used in command-based architectures to encapsulate the handling of application
    /// requests.</remarks>
    /// <typeparam name="TCommand">The type of command to handle. Must implement the <see cref="ICommand"/> interface.</typeparam>
    
    public interface ICommandHandler<in TCommand> 
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }

    /// <summary>
    /// Defines a contract for handling a command asynchronously and returning a response of the specified type.
    /// </summary>
    /// <remarks>This interface is typically used in command processing or CQRS patterns to decouple command invocation from
    /// command handling logic.</remarks>
    /// <typeparam name="TCommand">The type of command to handle. Must implement <see cref="ICommand{TResponse}"/>.</typeparam>
    /// <typeparam name="TResponse">The type of response returned after handling the command.</typeparam>
    
    public interface ICommandHandler<in TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<TResponse> HandleAsync(TCommand command);
    }
}
