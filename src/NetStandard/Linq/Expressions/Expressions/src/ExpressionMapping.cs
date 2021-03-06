﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public readonly struct ExpressionMapping
    {
        public readonly Expression SourceExpression;
        public readonly Expression TargetExpression;

        public ExpressionMapping(Expression source, Expression target)
        {
            SourceExpression = source;
            TargetExpression = target;
        }
    }
}
