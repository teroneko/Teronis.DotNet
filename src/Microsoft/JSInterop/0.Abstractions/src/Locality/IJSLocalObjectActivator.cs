﻿using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public interface IJSLocalObjectActivator : IInstanceActivator<IJSLocalObject>
    {
        IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference);
        ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName);
        ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string objectName);
    }
}