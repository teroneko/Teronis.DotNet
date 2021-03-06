﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks.Sources;

namespace Teronis.Microsoft.JSInterop.Threading.Tasks
{
    public class ExceptionValueTaskSource<T> : ExceptionValueTaskSource, IValueTaskSource<T>
    {
        public ExceptionValueTaskSource(Exception exception)
            : base(exception) { }

        T IValueTaskSource<T>.GetResult(short token) =>
            throw Exception;

        void IValueTaskSource<T>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        { }
    }
}
