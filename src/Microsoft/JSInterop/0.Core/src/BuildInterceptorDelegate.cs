﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public delegate IJSInterceptor BuildInterceptorDelegate(
        Action<IJSInterceptorBuilder>? configureBuilder);
}
