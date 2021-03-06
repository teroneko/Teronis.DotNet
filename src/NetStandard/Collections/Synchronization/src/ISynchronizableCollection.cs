﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    internal interface ICollectionSynchronizationContext<TItem>
    {
        void BeginCollectionSynchronization();
        void ProcessModification(ICollectionModification<TItem, TItem> superItemModification);
        void EndCollectionSynchronization();
    }
}
