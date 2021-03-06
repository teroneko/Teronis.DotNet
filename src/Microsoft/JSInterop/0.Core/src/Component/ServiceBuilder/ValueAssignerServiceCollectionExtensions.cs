﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Component.Assigners;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public static class ValueAssignerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default property assigners that resides in this namespace.
        /// </summary>
        /// <param name="propertyAssignerServices"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection AddNonDynamicDefaultValueAssigners(this ValueAssignerServiceCollection propertyAssignerServices)
        {
            propertyAssignerServices.UseExtension(extension => {
                extension.AddScoped<JSGlobalObjectActivatingAssigner>();
                extension.AddScoped<JSModuleActivatingAssigner>();
            });

            return propertyAssignerServices;
        }

        /// <summary>
        /// Adds <see cref="JSCustomFacadeWrappingAssigner"/>.
        /// </summary>
        /// <param name="propertyAssignerServices"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection AddJSCustomFacadeAssigner(this ValueAssignerServiceCollection propertyAssignerServices)
        {
            propertyAssignerServices.UseExtension(extension => extension.AddScoped<JSCustomFacadeWrappingAssigner>());
            return propertyAssignerServices;
        }

        /// <summary>
        /// Remves first occurrence of <see cref="JSCustomFacadeWrappingAssigner"/>.
        /// </summary>
        /// <param name="propertyAssignerServices"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection RemoveJSCustomFacadeAssigner(this ValueAssignerServiceCollection propertyAssignerServices)
        {
            propertyAssignerServices.UseExtension(extension => extension.RemoveAll<JSCustomFacadeWrappingAssigner>());
            return propertyAssignerServices;
        }
    }
}
