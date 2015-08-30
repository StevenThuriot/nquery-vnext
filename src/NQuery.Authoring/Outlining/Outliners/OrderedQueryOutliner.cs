using System;
using System.Collections.Generic;

using NQuery.Syntax;

namespace NQuery.Authoring.Outlining.Outliners
{
    public sealed class OrderedQueryOutliner : SyntaxNodeOutliner<OrderedQuerySyntax>
    {
        protected override IEnumerable<OutliningRegionSpan> FindRegions(OrderedQuerySyntax node)
        {
            var text = node.Query is SelectQuerySyntax ? "SELECT" : "...";
            yield return new OutliningRegionSpan(node.Span, text);
        }
    }
}