﻿using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Dynamic.Locality
{
    public interface IJSDynamicLocalObjectActivator : IInstanceActivator<IJSLocalObject>
    {
        ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, string objectName);
        ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, IJSObjectReference jsObjectReference, string objectName);
    }
}
