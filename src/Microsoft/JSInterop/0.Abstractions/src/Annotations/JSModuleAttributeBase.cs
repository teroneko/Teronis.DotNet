﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facade;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    public abstract class JSModuleAttributeBase : Attribute
    {
        /// <summary>
        /// The module name or path of a JavaScript ESM module relative to "wwwroot" (if not changed).
        /// </summary>
        public string? NameOrPath { get; set; }

        /// <summary>
        /// The properties (owned by component - see 
        /// <see cref="IJSFacadeHubActivator.CreateInstanceAsync(object)"/>) of type <see cref="IJSObjectReference"/>
        /// gets initialized when decorated with this attribute.
        /// </summary>
        public JSModuleAttributeBase() { }
    }
}
