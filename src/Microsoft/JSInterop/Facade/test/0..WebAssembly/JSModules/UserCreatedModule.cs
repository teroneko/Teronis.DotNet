﻿using Teronis.Microsoft.JSInterop.Facade;

namespace Teronis_._Microsoft.JSInterop.Facade.JSModules
{
    public class UserCreatedModule : ServiceProviderCreatedModule
    {
        public UserCreatedModule(IJSLocalObject objectReference)
            : base(objectReference) { }
    }
}