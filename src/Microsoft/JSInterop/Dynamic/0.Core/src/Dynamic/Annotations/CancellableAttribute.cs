﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Dynamic.Annotations
{
    /// <summary>
    /// Can be used to annotate a parameter of type
    /// <see cref="CancellableAttribute"/> or
    /// <see cref="TimeSpan"/>.
    /// The argument will then not be included in the
    /// final JavaScript argument list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CancellableAttribute : Attribute
    { }
}
