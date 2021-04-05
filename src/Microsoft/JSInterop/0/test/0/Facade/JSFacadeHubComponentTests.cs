﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Activators;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Modules;
using Teronis.Microsoft.JSInterop.ObjectReferences;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadeHubComponentTests
    {
        [AssignModule("module")]
        private IJSModule module { get; set; } = null!;

        [AssignModule("module"), AssignCustomFacade]
        CustomModule customModule = null!;

        [AssignGlobalObject("window")]
        IJSLocalObject window { get; set; } = null!;

        [Fact]
        public async Task Should_initalize_component()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IJSModuleActivator, EmptyModuleActivator>()
                .AddSingleton<IJSLocalObjectActivator, EmptyLocalObjectActivator>()
                .AddJSCustomFacadeActivator(configureOptions: options =>
                    options.JSFacadeDictionaryBuilder.Add<CustomModule>())
                .AddJSFacadeHub()
                .BuildServiceProvider();

            await using var facadeHub = await serviceProvider
                .GetRequiredService<IJSFacadeHubActivator>()
                .CreateInstanceAsync<JSFacadeActivators>(this);

            Assert.Equal("module", module.ModuleNameOrPath);
            Assert.Equal("module", customModule.Module.ModuleNameOrPath);

            var typedWindow = Assert.IsType<EmptyObjectReference>(window.ObjectReference);
            Assert.Equal(nameof(EmptyLocalObjectActivator), typedWindow.Tag);
        }
    }
}