﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public class SingletonServiceCollection : LifetimeServiceCollection<IServiceProvider, SingletonServiceDescriptor>, ISingletonServiceCollection
    {
        public override ServiceLifetime Lifetime =>
            ServiceLifetime.Singleton;
    }
}
