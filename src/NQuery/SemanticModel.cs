﻿using System;
using System.Collections.Generic;
using System.Linq;

using NQuery.Binding;
using NQuery.BoundNodes;
using NQuery.Symbols;

namespace NQuery
{
    public sealed class SemanticModel
    {
        private readonly Compilation _compilation;
        private readonly BindingResult _bindingResult;

        internal SemanticModel(Compilation compilation, BindingResult bindingResult)
        {
            _compilation = compilation;
            _bindingResult = bindingResult;
        }

        public Compilation Compilation
        {
            get { return _compilation; }
        }

        public Conversion ClassifyConversion(Type sourceType, Type targetType)
        {
            return Conversion.Classify(sourceType, targetType);
        }

        public Symbol GetSymbol(ExpressionSyntax expression)
        {
            var boundExpression = GetBoundExpression(expression);
            return boundExpression == null ? null : GetSymbol(boundExpression);
        }

        private static Symbol GetSymbol(BoundExpression expression)
        {
            switch (expression.Kind)
            {
                case BoundNodeKind.NameExpression:
                    return GetSymbol((BoundNameExpression) expression);
                case BoundNodeKind.VariableExpression:
                    return GetSymbol((BoundVariableExpression) expression);
                case BoundNodeKind.FunctionInvocationExpression:
                    return GetSymbol((BoundFunctionInvocationExpression) expression);
                case BoundNodeKind.AggregateExpression:
                    return GetSymbol((BoundAggregateExpression) expression);
                case BoundNodeKind.PropertyAccessExpression:
                    return GetSymbol((BoundPropertyAccessExpression) expression);
                case BoundNodeKind.MethodInvocationExpression:
                    return GetSymbol(((BoundMethodInvocationExpression) expression));
                default:
                    return null;
            }
        }

        private static Symbol GetSymbol(BoundNameExpression expression)
        {
            return expression.Symbol;
        }

        private static Symbol GetSymbol(BoundVariableExpression expression)
        {
            return expression.Symbol;
        }

        private static Symbol GetSymbol(BoundFunctionInvocationExpression expression)
        {
            return expression.Symbol;
        }

        private static Symbol GetSymbol(BoundAggregateExpression expression)
        {
            return expression.Symbol;
        }

        private static Symbol GetSymbol(BoundPropertyAccessExpression expression)
        {
            return expression.Symbol;
        }

        private static Symbol GetSymbol(BoundMethodInvocationExpression expression)
        {
            return expression.Symbol;
        }

        public Type GetExpressionType(ExpressionSyntax expression)
        {
            var boundExpression = GetBoundExpression(expression);
            return boundExpression == null ? null : boundExpression.Type;
        }

        public Conversion GetConversion(CastExpressionSyntax expression)
        {
            var boundExpression = GetBoundExpression(expression) as BoundCastExpression;
            return boundExpression == null ? null : boundExpression.Conversion;
        }

        private BoundExpression GetBoundExpression(ExpressionSyntax expression)
        {
            return _bindingResult.GetBoundNode(expression) as BoundExpression;
        }

        public IEnumerable<TableInstanceSymbol> GetDeclaredSymbols(TableReferenceSyntax tableReference)
        {
            var result = _bindingResult.GetBoundNode(tableReference) as BoundTableReference;
            return result == null ? null : result.GetDeclaredTableInstances();
        }

        public CommonTableExpressionSymbol GetDeclaredSymbol(CommonTableExpressionSyntax commonTableExpression)
        {
            var result = _bindingResult.GetBoundNode(commonTableExpression) as BoundCommonTableExpression;
            return result == null ? null : result.TableSymbol;
        }

        public TableInstanceSymbol GetDeclaredSymbol(TableReferenceSyntax tableReference)
        {
            var result = _bindingResult.GetBoundNode(tableReference) as BoundNamedTableReference;
            return result == null ? null : result.TableInstance;
        }

        public Symbol GetDeclaredSymbol(DerivedTableReferenceSyntax tableReference)
        {
            var result = _bindingResult.GetBoundNode(tableReference) as BoundDerivedTableReference;
            return result == null ? null : result.TableInstance;
        }

        public IEnumerable<Diagnostic> GetDiagnostics()
        {
            return _bindingResult.Diagnostics;
        }

        public IEnumerable<Symbol> LookupSymbols(int position)
        {
            var node = FindClosestNodeWithBinder(_bindingResult.Root, position);
            var binder = node == null ? null : _bindingResult.GetBinder(node);
            return binder == null
                       ? Enumerable.Empty<Symbol>()
                       : LookupSymbols(binder);
        }

        private static IEnumerable<Symbol> LookupSymbols(Binder binder)
        {
            // NOTE: We want to only show the *available* symbols. That means, we need
            //       hide symbols from the parent binder that have same name as the ones
            //       from a nested binder.
            //
            //       We do this by simply recording which names we've already seen.
            //       Please note that we *do* want to see duplicate names within the
            //       *same* binder.

            var allNames = new HashSet<string>();

            while (binder != null)
            {
                var localNames = new HashSet<string>();

                foreach (var symbol in binder.GetLocalSymbols())
                {
                    if (!allNames.Contains(symbol.Name))
                    {
                        localNames.Add(symbol.Name);
                        yield return symbol;
                    }
                }

                allNames.UnionWith(localNames);
                binder = binder.Parent;
            }
        }

        private SyntaxNode FindClosestNodeWithBinder(SyntaxNode root, int position)
        {
            var token = root.FindTokenContext(position);
            return (from n in token.Parent.AncestorsAndSelf()
                    let bc = _bindingResult.GetBinder(n)
                    where bc != null
                    select n).FirstOrDefault();
        }

        public IEnumerable<MethodSymbol> LookupMethods(Type type)
        {
            // TODO: Should we cache them to ensure object identity for method symbols?
            var dataContext = _compilation.DataContext;
            var methodProvider = dataContext.MethodProviders.LookupValue(type);
            return methodProvider == null
                       ? Enumerable.Empty<MethodSymbol>()
                       : methodProvider.GetMethods(type);
        }

        public IEnumerable<PropertySymbol> LookupProperties(Type type)
        {
            // TODO: Should we cache them to ensure object identity for property symbols?
            var dataContext = _compilation.DataContext;
            var propertyProvider = dataContext.PropertyProviders.LookupValue(type);
            return propertyProvider == null
                       ? Enumerable.Empty<PropertySymbol>()
                       : propertyProvider.GetProperties(type);
        }
    }
}