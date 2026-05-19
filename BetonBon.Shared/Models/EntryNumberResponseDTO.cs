using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public sealed record EntryNumberResponseDTO
    {
        public required int EntryNumber { get; init; }
    }
}
