using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    public interface ICommand;

    public interface ICommand<out TResponse>;
}
