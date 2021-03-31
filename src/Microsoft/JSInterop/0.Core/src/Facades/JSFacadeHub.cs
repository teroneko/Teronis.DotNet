﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadeHub<TJSFacadeActivators> : IJSFacadeHub<TJSFacadeActivators>
        where TJSFacadeActivators : IJSFacadeActivators
    {
        public IReadOnlyList<IAsyncDisposable> Disposables =>
            disposables;

        public TJSFacadeActivators Activators { get; }

        private readonly List<IAsyncDisposable> disposables;
        private readonly IJSCustomFacadeActivator jsCustomFacadeActivator;

        public JSFacadeHub(
            IJSCustomFacadeActivator jsFacadeResolver,
            TJSFacadeActivators jsFacadeActivators)
        {
            disposables = new List<IAsyncDisposable>();
            jsCustomFacadeActivator = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
            Activators = jsFacadeActivators ?? throw new ArgumentNullException(nameof(Activators));
            jsFacadeActivators.HookAnyInstanceActivated(RegisterDisposable);
        }

        public IAsyncDisposable this[int index] =>
            disposables[index];

        protected virtual void RegisterDisposable(IAsyncDisposable disposable) =>
            disposables.Add(disposable);

        protected internal void RegisterDisposables(IEnumerable<IAsyncDisposable> disposables)
        {
            foreach (var disposableInstance in disposables) {
                RegisterDisposable(disposableInstance);
            }
        }

        public virtual IAsyncDisposable CreateCustomFacade(Func<TJSFacadeActivators, IJSObjectReferenceFacade> getCustomFacadeConstructorParameter, Type jsCustomFacadeType)
        {
            var customFacadeConstructorParameter = getCustomFacadeConstructorParameter(Activators);
            var jsCustomFacade = jsCustomFacadeActivator.CreateCustomFacade(customFacadeConstructorParameter, jsCustomFacadeType);
            RegisterDisposable(jsCustomFacade);
            return jsCustomFacade;
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (disposables is null) {
                return;
            }

            foreach (var moduleWrapper in disposables) {
                await moduleWrapper.DisposeAsync();
            }
        }

        public ValueTask DisposeAsync() =>
            DisposeAsyncCore();
    }
}