﻿using System;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSFacadeDictionaryBuilder
    {
        IJSFacadeDictionaryBuilder AddFacade<T>(JSFacadeCreatorDelegate<T> jsFacadeCreatorHandler) 
            where T : class, IAsyncDisposable;

        IJSFacadeDictionaryBuilder AddFacade<T>()
            where T : class, IAsyncDisposable;
    }
}