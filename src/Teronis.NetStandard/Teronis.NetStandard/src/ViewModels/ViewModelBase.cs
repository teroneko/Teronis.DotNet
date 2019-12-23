﻿using System.ComponentModel;
using System.Linq;
using Teronis.Data;
using Teronis.Extensions.NetStandard;
using Teronis.Reflection;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Teronis.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IHaveParents, IHaveKnownParents, IWorking, INotifyDataErrorInfo
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
        public event WantParentsEventHandler WantParents;

        [IgnoreEntityVariable]
        public DynamicParentResolver DynamicParentResolver { get; private set; }

        public bool IsWorking
            => workStatusPropertyChangedCache.CachedProperties.Values.Any(x => x.IsWorking);

        protected WorkStatus WorkStatus { get; private set; }

        private KnownParentsContainer knownParentsContainer;
        private PropertyChangedRelay propertyChangedRelay;
        private PropertyChangedCache<IHaveParents> havingParentsPropertyChangedCache;
        private PropertyChangedCache<IWorking> workStatusPropertyChangedCache;

        public ViewModelBase()
        {
            DynamicParentResolver = new DynamicParentResolver(this);
            knownParentsContainer = new KnownParentsContainer(this);
            propertyChangedRelay = new PropertyChangedRelay(this);
            propertyChangedRelay.NotifiersPropertyChanged += PropertyChangedRelay_NotifiersPropertyChanged;
            havingParentsPropertyChangedCache = new PropertyChangedCache<IHaveParents>(this);
            havingParentsPropertyChangedCache.PropertyCacheAdded += HavingParentsPropertyChangedCache_PropertyCacheAdded;
            havingParentsPropertyChangedCache.PropertyCacheRemoved -= HavingParentsPropertyChangedCache_PropertyCacheRemoved;
            /// We only subscribe to <see cref="IWorking"/>-container, so that we can on calculate
            /// <see cref="IsWorking"/> properly.
            workStatusPropertyChangedCache = new PropertyChangedCache<IWorking>(this);
            WorkStatus = new WorkStatus();
            validationErrors = new Dictionary<string, ICollection<string>>();
            validationErrorPreviews = new Dictionary<string, ICollection<string>>();
        }

        private void Property_WantParents(object sender, HavingParentsEventArgs havingParents)
            => havingParents.AttachParentParents(this);

        private void HavingParentsPropertyChangedCache_PropertyCacheAdded(object sender, PropertyCacheAddedEventArgs<IHaveParents> args)
            => args.AddedProperty.WantParents += Property_WantParents;

        private void HavingParentsPropertyChangedCache_PropertyCacheRemoved(object sender, PropertyCacheRemovedEventArgs<IHaveParents> args)
            => args.Property.WantParents -= Property_WantParents;

        protected void OnPropertyChanged(PropertyChangedEventArgs args)
            => PropertyChanged?.Invoke(this, args);

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            OnPropertyChanged(args);
        }

        private void PropertyChangedRelay_NotifiersPropertyChanged(object sender, PropertyChangedEventArgs args)
            => OnPropertyChanged(args);

        public ParentsPicker GetParentsPicker()
            => new ParentsPicker(this, WantParents);

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public virtual bool HasErrors
            => validationErrors.Count > 0;

        public virtual bool HasErrorPreviews
            => HasErrors || validationErrorPreviews.Count > 0;

        private Dictionary<string, ICollection<string>> validationErrors;
        private Dictionary<string, ICollection<string>> validationErrorPreviews;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !validationErrors.ContainsKey(propertyName))
                return null;
            else
                return validationErrors[propertyName];
        }

        private void onErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

        private void onErrorPreviewsChanged(string propertyName)
            => OnPropertyChanged(nameof(HasErrorPreviews));

        protected void SetErrors(string propertyName, ICollection<string> errors, bool isPreview)
        {
            if (!isPreview)
                validationErrors[propertyName] = errors;

            validationErrorPreviews[propertyName] = errors;

            if (!isPreview)
                onErrorsChanged(propertyName);

            onErrorPreviewsChanged(propertyName);
        }

        protected void RemoveErrors(string propertyName, bool isPreview)
        {
            if (!isPreview && validationErrors.ContainsKey(propertyName))
                validationErrors.Remove(propertyName);

            if (validationErrorPreviews.ContainsKey(propertyName))
                validationErrorPreviews.Remove(propertyName);

            if (!isPreview)
                onErrorsChanged(propertyName);

            onErrorPreviewsChanged(propertyName);
        }

        #endregion

        #region IHaveKnownParents

        public void AttachKnownWantParentsHandler(object caller, WantParentsEventHandler handler)
            => knownParentsContainer.AttachWantParentsHandler(caller, handler);

        public void AttachWantParentsHandler(WantParentsEventHandler handler)
            => WantParents += handler;

        public void DetachKnownWantParentsHandler(object caller)
            => knownParentsContainer.DetachWantParentsHandler(caller);

        public void DetachWantParentsHandler(WantParentsEventHandler handler)
            => WantParents -= handler;

        #endregion

        #region IWorking

        public virtual void BeginWork()
            => WorkStatus.BeginWork();

        public virtual void EndWork()
            => WorkStatus.EndWork();

        #endregion
    }
}