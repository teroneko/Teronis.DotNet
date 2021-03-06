﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Threading;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;
using Teronis.Microsoft.JSInterop.Dynamic.Reflection;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    internal class CancellableAnnotatedParameter : ParameterBase<CancellableAttribute>
    {
        public bool IsCancellationToken { get; private set; }
        public bool IsTimeout { get; private set; }

        public CancellableAnnotatedParameter(ParameterInfo parameterInfo, CancellableAttribute attribute)
            : base(parameterInfo, attribute) { }

        /// <summary>
        /// Reads parameter info.
        /// </summary>
        /// <exception cref="NotSupportedException">Invalid parameter type.</exception>
        protected internal override void ReadParameterInfo()
        {
            if (ParameterInfo.ParameterType == typeof(CancellationToken)) {
                IsCancellationToken = true;
            } else if (ParameterInfo.ParameterType == typeof(TimeSpan)) {
                IsTimeout = true;
            } else {
                throw new NotSupportedException($"The parameter type can only of type {typeof(CancellationToken)} and {typeof(TimeSpan)}.");
            }
        }
    }
}
