﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Mvc.ApplicationModels
{
    public static class IServiceCollectionExtensions
    {
        public static void ApplyControllerModelConfiguration(this IServiceCollection services, ControllerModelConfiguration controllerModelConfiguration,
            Action<IServiceCollection, Action<MvcOptions>> configureOptions) =>
            configureOptions(services, new MvcOptionsConfigurator(controllerModelConfiguration).ConfigureMvcOptions);

        public static void ApplyControllerModelConfiguration(this IServiceCollection _, ControllerModelConfiguration controllerModelConfiguration,
            Action<Action<MvcOptions>> configureOptions) =>
            configureOptions(new MvcOptionsConfigurator(controllerModelConfiguration).ConfigureMvcOptions);

        public static void ApplyControllerModelConfiguration(this IServiceCollection services, ControllerModelConfiguration controllerModelConfiguration) =>
            ApplyControllerModelConfiguration(services, controllerModelConfiguration, configurator => services.Configure(configurator));

        private class MvcOptionsConfigurator
        {
            private readonly ControllerModelConfiguration controllerModelConfiguration;

            public MvcOptionsConfigurator(ControllerModelConfiguration controllerModelConfiguration) =>
                this.controllerModelConfiguration = controllerModelConfiguration;

            public void ConfigureMvcOptions(MvcOptions options)
            {
                foreach (var controllerConvention in controllerModelConfiguration.ControllerConventions) {
                    options.Conventions.Add(controllerConvention);
                }

                foreach (var actionConvention in controllerModelConfiguration.ActionConventions) {
                    options.Conventions.Add(actionConvention);
                }

                foreach (var parameterConvention in controllerModelConfiguration.ParameterConventions) {
                    options.Conventions.Add(parameterConvention);
                }

                foreach (var parameterBaseConvention in controllerModelConfiguration.ParameterBaseConventions) {
                    options.Conventions.Add(parameterBaseConvention);
                }
            }
        }
    }
}