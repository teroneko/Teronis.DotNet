﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public delegate void PostConfigureInterceptorServicesOptionsDelegate<TInterceptorServicesOptions, TDerivedValueAssignerServicesOptions>(
        TInterceptorServicesOptions interceptorServicesOptions,
        TDerivedValueAssignerServicesOptions propertyAssignerServicesOptions)
        where TInterceptorServicesOptions : JSInterceptorServicesOptions<TInterceptorServicesOptions>
        where TDerivedValueAssignerServicesOptions : ValueAssignerServicesOptions<TDerivedValueAssignerServicesOptions>;
}
