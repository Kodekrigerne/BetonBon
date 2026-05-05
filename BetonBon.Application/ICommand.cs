using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    // Mikkel Klitgaard

    /// <summary>
    /// Defines a contract for an executable command.
    /// </summary>
    /// <remarks>Implement this interface to represent an action or operation that can be invoked, typically
    /// in command pattern scenarios such as UI frameworks or task execution pipelines.</remarks>
    
    public interface ICommand;

    /// <summary>
    /// Represents a command that returns a response of the specified type when executed.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
    
    public interface ICommand<out TResponse>;
}
