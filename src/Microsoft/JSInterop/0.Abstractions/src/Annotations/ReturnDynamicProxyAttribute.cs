﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    /// <summary>
    /// This attribute should only applied on a method in an interface that
    /// is used as proxy. By using this attribute we are definitely expecting
    /// a JavaScript object reference when calling
    /// <see cref="IJSObjectInvocationBase{ReturnType}.GetDeterminedResult()"/>.
    /// Used in interceptor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnDynamicProxyAttribute : Attribute
    { 
        public Type? InterfaceToBeProxied { get; set; }
    }
}
