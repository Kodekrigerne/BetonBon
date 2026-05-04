using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    public interface ICommandHandler<in TCommand> 
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }


    public interface ICommandHandler<in TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<TResponse> HandleAsync(TCommand command);
    }
}
