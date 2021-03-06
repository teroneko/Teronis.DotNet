﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Component.Assigners;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDerived">The derived type. When registered to service provider it provides uniqueness.</typeparam>
    public class JSInterceptorServicesOptions<TDerived>
        where TDerived : JSInterceptorServicesOptions<TDerived>
    {
        public IValueAssignerServicesOptions ValueAssignerServicesOptions {
            get {
                if (propertyAssignerServicesOptions is null) {
                    throw new InvalidOperationException("The property assigner options has not been set up.");
                }

                return propertyAssignerServicesOptions;
            }
        }

        /// <summary>
        /// A list of <see cref="IValueAssigner"/> that is lazy initialized.
        /// </summary>
        internal IValueAssignerList ValueAssigners {
            get {
                if (propertyAssigners is null) {
                    throw new InvalidOperationException("The property assigners has not been set up.");
                }

                return propertyAssigners;
            }
        }

        internal bool AreInterceptorServicesUserTouched { get; private set; }

        internal virtual JSInterceptorServiceCollection InterceptorServices {
            get {
                if (interceptorServices is null) {
                    interceptorServices = new JSInterceptorServiceCollection();
                    AreInterceptorServicesUserTouched = true;
                }

                return interceptorServices;
            }
        }

        private JSInterceptorServiceCollection? interceptorServices;
        private IValueAssignerServicesOptions? propertyAssignerServicesOptions;
        private IValueAssignerList? propertyAssigners;

        internal void SetValueAssignerServicesOptions(IValueAssignerServicesOptions propertyAssignerServicesOptions) =>
            this.propertyAssignerServicesOptions = propertyAssignerServicesOptions ?? throw new ArgumentNullException(nameof(propertyAssignerServicesOptions));

        internal void SetValueAssignerList(IValueAssignerList propertyAssigners) =>
            this.propertyAssigners = propertyAssigners ?? throw new ArgumentNullException(nameof(propertyAssigners));

        internal bool CreateInterceptorServicesWhenUserUntouched()
        {
            if (AreInterceptorServicesUserTouched) {
                return false;
            }

            if (interceptorServices is null) {
                interceptorServices = new JSInterceptorServiceCollection();
            }

            return true;
        }

        /// <summary>
        /// Configures an implementation of <see cref="IJSInterceptorServiceCollection"/>.
        /// </summary>
        /// <param name="callback"></param>
        public void ConfigureInterceptorServices(Action<IJSInterceptorServiceCollection> callback) =>
            callback?.Invoke(InterceptorServices);
    }
}
