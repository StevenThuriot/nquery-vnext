#nullable enable

using System;

namespace NQuery
{
    public enum SyntaxKind
    {
        // Tokens

        EndOfFileToken,
        BadToken,

        IdentifierToken,
        NumericLiteralToken,
        StringLiteralToken,
        DateLiteralToken,
        AtToken,

        BitwiseNotToken,
        AmpersandToken,
        BarToken,
        CaretToken,
        LeftParenthesisToken,
        RightParenthesisToken,
        PlusToken,
        MinusToken,
        AsteriskToken,
        SlashToken,
        PercentToken,
        AsteriskAsteriskToken,
        CommaToken,
        DotToken,
        EqualsToken,
        ExclamationEqualsToken,
        LessGreaterToken,
        LessToken,
        LessEqualToken,
        GreaterToken,
        GreaterEqualToken,
        ExclamationLessToken,
        ExclamationGreaterToken,
        GreaterGreaterToken,
        LessLessToken,

        // Keywords

        AndKeyword,
        OrKeyword,
        IsKeyword,
        NullKeyword,
        NotKeyword,
        LikeKeyword,
        SoundsKeyword,
        SimilarKeyword,
        BetweenKeyword,
        InKeyword,
        CastKeyword,
        AsKeyword,
        CoalesceKeyword,
        NullIfKeyword,
        CaseKeyword,
        WhenKeyword,
        ThenKeyword,
        ElseKeyword,
        EndKeyword,
        TrueKeyword,
        FalseKeyword,
        ToKeyword,

        // Contextual keywords

        SelectKeyword,
        TopKeyword,
        DistinctKeyword,
        FromKeyword,
        WhereKeyword,
        GroupKeyword,
        ByKeyword,
        HavingKeyword,
        OrderKeyword,
        AscKeyword,
        DescKeyword,
        UnionKeyword,
        AllKeyword,
        IntersectKeyword,
        ExceptKeyword,
        ExistsKeyword,
        AnyKeyword,
        SomeKeyword,
        JoinKeyword,
        InnerKeyword,
        CrossKeyword,
        LeftKeyword,
        RightKeyword,
        OuterKeyword,
        FullKeyword,
        OnKeyword,
        WithKeyword,
        TiesKeyword,
        RecursiveKeyword,

        // Trivia

        WhitespaceTrivia,
        EndOfLineTrivia,
        MultiLineCommentTrivia,
        SingleLineCommentTrivia,
        SkippedTokensTrivia,

        // UnaryExpressions

        ComplementExpression,
        IdentityExpression,
        NegationExpression,
        LogicalNotExpression,

        // Nodes

        CompilationUnit,

        // Binary expressions

        BitwiseAndExpression,
        BitwiseOrExpression,
        ExclusiveOrExpression,
        AddExpression,
        SubExpression,
        MultiplyExpression,
        DivideExpression,
        ModuloExpression,
        PowerExpression,
        EqualExpression,
        NotEqualExpression,
        LessExpression,
        LessOrEqualExpression,
        GreaterExpression,
        GreaterOrEqualExpression,
        NotLessExpression,
        NotGreaterExpression,
        LeftShiftExpression,
        RightShiftExpression,
        LogicalAndExpression,
        LogicalOrExpression,
        LikeExpression,
        SoundsLikeExpression,
        SimilarToExpression,

        // Expressions

        ParenthesizedExpression,
        BetweenExpression,
        IsNullExpression,
        CastExpression,
        CaseExpression,
        CaseLabel,
        CaseElseLabel,
        CoalesceExpression,
        NullIfExpression,
        InExpression,
        InQueryExpression,
        LiteralExpression,
        VariableExpression,
        NameExpression,
        PropertyAccessExpression,
        CountAllExpression,
        FunctionInvocationExpression,
        MethodInvocationExpression,
        ArgumentList,

        SingleRowSubselect,
        ExistsSubselect,
        AllAnySubselect,

        // Queries

        ParenthesizedTableReference,
        NamedTableReference,
        CrossJoinedTableReference,
        InnerJoinedTableReference,
        OuterJoinedTableReference,
        DerivedTableReference,

        ExceptQuery,
        UnionQuery,
        IntersectQuery,
        OrderedQuery,
        OrderByColumn,
        ParenthesizedQuery,
        CommonTableExpressionQuery,
        CommonTableExpression,
        CommonTableExpressionColumnName,
        CommonTableExpressionColumnNameList,
        SelectQuery,
        TopClause,
        WildcardSelectColumn,
        ExpressionSelectColumn,
        SelectClause,
        FromClause,
        WhereClause,
        GroupByClause,
        GroupByColumn,
        HavingClause,
        Alias,
    }
}