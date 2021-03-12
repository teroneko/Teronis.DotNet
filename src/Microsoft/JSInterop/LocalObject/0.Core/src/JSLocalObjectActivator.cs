﻿using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.LocalObject
{
    public class JSLocalObjectActivator : IJSLocalObjectActivator
    {
        private readonly IJSLocalObjectInterop jsLocalObjectInterop;
        private readonly IJSFunctionalObjectReference jsFunctionalObjectReference;

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop, JSLocalObjectActivatorOptions? options)
        {
            this.jsLocalObjectInterop = jsLocalObjectInterop ?? throw new System.ArgumentNullException(nameof(jsLocalObjectInterop));
            jsFunctionalObjectReference = options?.GetOrBuildJSFunctionalObjectReference() ?? JSFunctionalObjectReference.Default;
        }

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop)
            : this(jsLocalObjectInterop, options: null) { }

        public IJSLocalObject CreateLocalObject(IJSObjectReference jsObjectReference) =>
            new JSLocalObject(jsFunctionalObjectReference, jsObjectReference, this);

        public async ValueTask<IJSLocalObject> CreateLocalObjectAsync(string objectName) =>
            CreateLocalObject(await jsLocalObjectInterop.CreateObjectReferenceAsync(objectName));

        public async ValueTask<IJSLocalObject> CreateLocalObjectAsync(IJSObjectReference objectReference, string objectName)
        {
            var nestedObjectReference = await jsLocalObjectInterop.CreateObjectReferenceAsync(objectReference, objectName);
            return CreateLocalObject(nestedObjectReference);
        }

        public ValueTask<IJSLocalObject> CreateLocalObjectAsync(IJSLocalObject jsLocalObject, string objectName) =>
            CreateLocalObjectAsync(jsLocalObject.JSObjectReference, objectName);
    }
}
