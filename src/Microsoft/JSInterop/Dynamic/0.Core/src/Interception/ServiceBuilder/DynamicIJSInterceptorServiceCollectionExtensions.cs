﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Interception.Interceptors;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public static class DynamicIJSInterceptorServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default dynamic-only interceptors.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorServiceCollection AddDefaultDynamicInterceptors(this IJSInterceptorServiceCollection interceptorBuilder)
        {
            interceptorBuilder.UseExtension(extension => {
                extension.AddScoped<JSDynamicLocalObjectActivatingInterceptor>();
                extension.AddScoped<JSDynamicLocalObjectReturningInterceptor>();
                extension.AddScoped<JSDynamicProxyReturningInterceptor>();
            });

            return interceptorBuilder;
        }
    }
}
