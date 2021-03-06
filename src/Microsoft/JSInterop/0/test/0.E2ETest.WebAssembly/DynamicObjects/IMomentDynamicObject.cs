﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis_._Microsoft.JSInterop.DynamicObjects
{
    public interface IMomentDynamicObject : IJSLocalObject
    {
        ValueTask<string> format();
    }
}
