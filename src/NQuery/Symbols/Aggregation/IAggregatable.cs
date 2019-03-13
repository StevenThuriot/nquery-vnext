#nullable enable

using System;

namespace NQuery.Symbols.Aggregation
{
    public interface IAggregatable
    {
        IAggregator CreateAggregator();
        Type ReturnType { get; }
    }
}