﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    /// <summary>
    /// Used in interceptor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnLocalObjectAttribute : LocalObjectAttributeBase
    {  }
}
