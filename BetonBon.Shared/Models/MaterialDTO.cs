using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public sealed record MaterialDTO
    {
        public required int Number { get; init; }
        public required string Name { get; init; }
    }
}
