﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObject : JSObjectReferenceFacade, IJSLocalObject
    {
        public JSLocalObject(IJSObjectReference jsObjectReference, IJSInterceptor? jsInterceptor)
            : base(jsObjectReference, jsInterceptor) { }
    }
}
