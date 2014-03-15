﻿using System;
using System.Collections.Generic;

using NQuery.Syntax;
using NQuery.Text;

namespace NQuery.Authoring.CodeActions.Issues
{
    internal sealed class ComparisonWithNullCodeIssueProvider : CodeIssueProvider<BinaryExpressionSyntax>
    {
        protected override IEnumerable<CodeIssue> GetIssues(SemanticModel semanticModel, BinaryExpressionSyntax node)
        {
            var leftIsNull = IsNullLiteral(node.Left);
            var rightIsNull = IsNullLiteral(node.Right);
            if (!leftIsNull && !rightIsNull)
                yield break;

            var isEquals = node.OperatorToken.Kind == SyntaxKind.EqualsToken;
            var isNotEquals = node.OperatorToken.Kind == SyntaxKind.ExclamationEqualsToken ||
                              node.OperatorToken.Kind == SyntaxKind.LessGreaterToken;
            var isComparison = isEquals || isNotEquals;

            if (!isComparison)
            {
                yield return new CodeIssue(CodeIssueKind.Warning, node.Span, "Expression is always NULL");
            }
            else
            {
                var action = new[] {new ConvertToIsNullCodeAction(node, isEquals)};
                yield return new CodeIssue(CodeIssueKind.Warning, node.Span, "Comparison with NULL results in NULL", action);
            }
        }

        private static bool IsNullLiteral(ExpressionSyntax node)
        {
            var literal = node as LiteralExpressionSyntax;
            return literal != null && literal.Token.Kind == SyntaxKind.NullKeyword;
        }

        private sealed class ConvertToIsNullCodeAction : ICodeAction
        {
            private readonly BinaryExpressionSyntax _node;
            private readonly bool _isEquals;

            public ConvertToIsNullCodeAction(BinaryExpressionSyntax node, bool isEquals)
            {
                _node = node;
                _isEquals = isEquals;
            }

            public string Description
            {
                get { return _isEquals ? "Convert to IS NULL" : "Convert to IS NOT NULL"; }
            }

            public SyntaxTree GetEdit()
            {
                var newText = _isEquals ? " IS NULL" : " IS NOT NULL";
                var syntaxTree = _node.SyntaxTree;

                var useLeft = IsNullLiteral(_node.Right);
                if (useLeft)
                {
                    var replacementSpan = TextSpan.FromBounds(_node.Left.Span.End, _node.Span.End);
                    return syntaxTree.ReplaceText(replacementSpan, newText);
                }
                
                var removalSpan = TextSpan.FromBounds(_node.Span.Start, _node.Right.Span.Start);
                var insertionPoint = _node.Right.Span.End - removalSpan.Length;
                return syntaxTree.RemoveText(removalSpan).InsertText(insertionPoint, newText);
            }
        }
    }
}