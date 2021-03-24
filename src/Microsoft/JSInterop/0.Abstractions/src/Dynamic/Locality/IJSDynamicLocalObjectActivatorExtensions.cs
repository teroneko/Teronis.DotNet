﻿using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Dynamic.Locality
{
    public static class IJSDynamicLocalObjectActivatorExtensions
    {
        public static async ValueTask<T> CreateInstanceAsync<T>(this IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator, string moduleNameOrPath)
            where T : class, IJSLocalObject =>
            (T)await jsDynamicLocalObjectActivator.CreateInstanceAsync(typeof(T), moduleNameOrPath);

        public static async ValueTask<T> CreateInstanceAsync<T>(this IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator, IJSObjectReference jsObjectReference, string objectName)
            where T : class, IJSLocalObject =>
            (T)await jsDynamicLocalObjectActivator.CreateInstanceAsync(typeof(T), jsObjectReference, objectName);

        public static ValueTask<T> CreateInstanceAsync<T>(this IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator, IJSLocalObject jsLocalObject, string objectName)
            where T : class, IJSLocalObject =>
            jsDynamicLocalObjectActivator.CreateInstanceAsync<T>(jsLocalObject.JSObjectReference, objectName);
    }
}