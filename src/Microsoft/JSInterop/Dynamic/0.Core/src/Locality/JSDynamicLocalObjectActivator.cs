﻿using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Internals.Utils;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Dynamic.Locality
{
    public class JSDynamicLocalObjectActivator : IInstanceActivatorBase<IJSLocalObject>, IJSDynamicLocalObjectActivator
    {
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;
        private readonly IJSDynamicProxyActivator jSDynamicProxyActivator;

        public JSDynamicLocalObjectActivator(IJSLocalObjectActivator jsLocalObjectActivator, IJSDynamicProxyActivator jSDynamicProxyActivator)
        {
            this.jsLocalObjectActivator = jsLocalObjectActivator;
            this.jSDynamicProxyActivator = jSDynamicProxyActivator;
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, string objectName)
        {
            TypeUtils.EnsureInterfaceTypeIsAssignaleTo<IJSLocalObject>(interfaceToBeProxied);
            var jsLocalObject = await jsLocalObjectActivator.CreateInstanceAsync(objectName);
            var jsDynamicLocalObject = (IJSLocalObject)jSDynamicProxyActivator.CreateInstance(interfaceToBeProxied, jsLocalObject);
            DispatchInstanceActicated(jsDynamicLocalObject);
            return jsDynamicLocalObject;
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, IJSObjectReference jsObjectReference, string objectName)
        {
            TypeUtils.EnsureInterfaceTypeIsAssignaleTo<IJSLocalObject>(interfaceToBeProxied);
            var jsLocalObject = await jsLocalObjectActivator.CreateInstanceAsync(jsObjectReference, objectName);
            var jsDynamicLocalObject = (IJSLocalObject)jSDynamicProxyActivator.CreateInstance(interfaceToBeProxied, jsLocalObject);
            DispatchInstanceActicated(jsDynamicLocalObject);
            return jsDynamicLocalObject;
        }
    }
}