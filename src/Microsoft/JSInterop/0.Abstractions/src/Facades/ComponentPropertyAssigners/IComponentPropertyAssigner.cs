﻿using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners
{
    public interface IComponentPropertyAssigner
    {
        /// <summary>
        /// Assigns the component property with returning non-null JavaScript facade.
        /// </summary>
        /// <param name="componentProperty">The component property.</param>
        /// <returns>"Null"/default or the JavaScript facade.</returns>
        ValueTask<YetNullable<IAsyncDisposable>> TryAssignComponentProperty(IComponentProperty componentProperty);
    }
}