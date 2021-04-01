﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Component;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSDynamicModuleActivatingInterceptor : JSPropertyAssigningInterceptorBase<JSDynamicModulePropertyAssigner>
    {
        public JSDynamicModuleActivatingInterceptor(JSDynamicModulePropertyAssigner propertyAssigner)
            : base(propertyAssigner) { }
    }
}
