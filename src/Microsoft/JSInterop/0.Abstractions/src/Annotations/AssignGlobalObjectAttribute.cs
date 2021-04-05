﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public class AssignGlobalObjectAttribute : Attribute
    {
        public string? NameOrPath { get; set; }

        public AssignGlobalObjectAttribute()
        { }
    }
}