﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades.PropertyAssigners
{
    public class JSDynamicModulePropertyAssigner : IPropertyAssigner
    {
        private readonly IJSDynamicModuleActivator jsDynamicModuleActivator;

        public JSDynamicModulePropertyAssigner(IJSDynamicModuleActivator jsDynamicModuleActivator) =>
            this.jsDynamicModuleActivator = jsDynamicModuleActivator ?? throw new ArgumentNullException(nameof(jsDynamicModuleActivator));

        /// <summary>
        /// Assigns component property with returning non-null JavaScript module facade.
        /// </summary>
        /// <returns>null/default or the JavaScript module facade.</returns>
        public virtual async ValueTask<YetNullable<IAsyncDisposable>> TryAssignProperty(IComponentProperty componentProperty)
        {
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<JSDynamicModulePropertyAttribute, JSDynamicModuleClassAttribute>(
                componentProperty,
                out var moduleNameOrPath)) {
                return default;
            }

            var jsFacade = (IAsyncDisposable)await jsDynamicModuleActivator.CreateInstanceAsync(componentProperty.OrignatingType, moduleNameOrPath);
            return new YetNullable<IAsyncDisposable>(jsFacade);
        }
    }
}
