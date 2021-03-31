﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    public interface IDefinition : ICustomAttributes
    {
        Type MemberType { get; }
        IDefinitionMemberType DefinitionMemberType { get; }
    }
}
