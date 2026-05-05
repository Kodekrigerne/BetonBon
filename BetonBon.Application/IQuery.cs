using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Application
{
    /// <summary>
    /// Represents a query operation that returns a result of the specified type.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the query operation.</typeparam>
    
    public interface IQuery<out TResult>;
}
