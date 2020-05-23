﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Teronis.Data;
using Teronis.Extensions;
using Teronis.ObjectModel.Updates;

namespace Teronis.Collections.Synchronization
{
    public class CollectionItemUpdateBehaviour<ItemType, ContentType>
        where ItemType : IApplyContentUpdateBy<ContentType>
    {
        public INotifyCollectionChangeApplied<ItemType, ContentType> CollectionChangeAppliedNotifier { get; private set; }

        public CollectionItemUpdateBehaviour(INotifyCollectionChangeApplied<ItemType, ContentType> collectionChangeNotifier)
        {
            CollectionChangeAppliedNotifier = collectionChangeNotifier;
            CollectionChangeAppliedNotifier.CollectionChangeApplied += NotifiableCollectionContainer_CollectionChangeAppliedAsync;
        }

        private IEnumerable<UpdateWithTargetContainer<ContentType, ItemType>> getOldItemUpdateContainerIterator(ICollectionChange<ItemType, ContentType> change)
        {
            if (change.Action == NotifyCollectionChangedAction.Replace) {
                var oldItemsEnumerator = change.OldItems.GetEnumerator();
                var newItemsEnumerator = change.NewItems.GetEnumerator();

                while (oldItemsEnumerator.MoveNext() && newItemsEnumerator.MoveNext()) {
                    var oldItem = oldItemsEnumerator.Current;
                    var newItem = newItemsEnumerator.Current;
                    var oldItemUpdate = new ContentUpdate<ContentType>(newItem, this);

                    var oldItemUpdateContainer = new UpdateWithTargetContainer<ContentType, ItemType>() {
                        Update = oldItemUpdate,
                        Target = oldItem
                    };

                    yield return oldItemUpdateContainer;
                }
            }
        }

        private async Task updateOldItemsAsync(ICollectionChange<ItemType, ContentType> change)
        {
            foreach (var oldItemUpdateContainer in getOldItemUpdateContainerIterator(change))
                await oldItemUpdateContainer.Target.ApplyContentUpdateByAsync(oldItemUpdateContainer.Update);
        }

        private async void NotifiableCollectionContainer_CollectionChangeAppliedAsync(object sender, CollectionChangeAppliedEventArgs<ItemType, ContentType> args)
        {
            if (!args.IsAsyncEvent())
                return;

            var change = args.ItemContentChange;
            var tcs = args.AsyncEventSequence.RegisterDependency();

            try {
                await updateOldItemsAsync(change);
                tcs.SetResult();
            } catch (Exception error) {
                tcs.SetException(error);
            }
        }
    }
}