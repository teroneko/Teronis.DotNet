﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Teronis.Data;

namespace Teronis.Configuration
{
    public class SettingsPropertyCache<PropertyType> : ISettingsPropertyCache, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string PropertyName
            => property.Name;

        private object propertyValue {
            get => settings[PropertyName];
            set => settings[PropertyName] = value;
        }

        /// <summary>
        /// If <see cref="settings"/> does not implement
        /// <see cref="INotifyPropertyChanged"/> then you
        /// should NOT bind to this, as this instance
        /// won't know about changes made to this property
        /// in <see cref="settings"/>.
        /// </summary>
        public PropertyType PropertyValue {
            get => (PropertyType)propertyValue;
            set => propertyValue = value;
        }

        object ISettingsPropertyCache.PropertyValue {
            get => propertyValue;
            set => propertyValue = value;
        }

        public PropertyType CachedPropertyValue {
            get => cachedProperty;

            private set {
                cachedProperty = value;
                OnPropertyChanged();
                recalculateIsCopySynchronous();
            }
        }

        object ISettingsPropertyCache.CachedPropertyValue
            => CachedPropertyValue;

        public bool IsCacheSynchronous {
            get => isCopySynchronous;

            set {
                isCopySynchronous = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// You may refer to an method that notifies about changes of a 
        /// property of <see cref="settings"/> if it does not implement 
        /// <see cref="INotifyPropertyChanged"/> already.
        /// </summary>
        public Action<string> NotifySettingsAboutPropertyChange { get; set; }

        public IEqualityComparer<PropertyType> PropertyValueEqualityComparer { get; private set; }

        private PropertyType cachedProperty;
        private SettingsBase settings;
        private SettingsProperty property;
        private bool isCopySynchronous;
        private PropertyChangedCache<PropertyType> propertyChangedCache;

        public SettingsPropertyCache(SettingsBase settings, string name, IEqualityComparer<PropertyType> propertyValueEqualityComparer)
        {
            this.settings = settings
                ?? throw new ArgumentNullException(nameof(settings));

            property = settings.Properties[name]
                ?? throw new ArgumentException($"Property '{name}' is not member of {nameof(settings)}");

            var genericPropertyType = typeof(PropertyType);

            if (genericPropertyType != property.PropertyType && !typeof(PropertyType).IsAssignableFrom(property.PropertyType))
                throw new ArgumentException($"The types aren't the same");

            PropertyValueEqualityComparer = propertyValueEqualityComparer
                ?? EqualityComparer<PropertyType>.Default;

            RefreshCache(false); // Now we have the equality comparer

            if (settings is INotifyPropertyChanged settingsPropertyChangedNotifier)
            {
                propertyChangedCache = new PropertyChangedCache<PropertyType>(settingsPropertyChangedNotifier)
                {
                    CanSkipRemovedEventInvocationWhenRecaching = true, // We benefit from not calculating twice during recache (remove/add)
                    CanHandleDefaultValue = false
                };

                propertyChangedCache.PropertyCacheAdding += PropertyChangedCache_PropertyCacheAdding;
                propertyChangedCache.PropertyCacheAdded += PropertyChangedCache_PropertyCacheAdded;
                propertyChangedCache.PropertyCacheRemoved += PropertyChangedCache_PropertyCacheRemoved;
                propertyChangedCache.OnPropertyChangedNotifierPropertyChanged(PropertyName);

                if (propertyChangedCache.CachedProperties.Count == 0)
                    throw new Exception($"The property {PropertyName} has not been found");
            }
        }

        public SettingsPropertyCache(SettingsBase settings, string name)
            : this(settings, name, default) { }

        private void PropertyChangedCache_PropertyCacheAdding(object sender, PropertyCacheAddingEventArgs<PropertyType> args)
            => args.IsPropertyCacheable = args.PropertyName == PropertyName;

        private void recalculateIsCopySynchronous()
        {
            if (PropertyValueEqualityComparer == null)
                return;

            var isPropertyChangedCacheNotNull = propertyChangedCache != null;

            var doesCachedPropertyExist = isPropertyChangedCacheNotNull
                && propertyChangedCache.CachedProperties.ContainsKey(PropertyName);

            if (isPropertyChangedCacheNotNull && !doesCachedPropertyExist)
                IsCacheSynchronous = false;
            else
            {
                PropertyType cachedProperty;

                if (doesCachedPropertyExist)
                    cachedProperty = propertyChangedCache.CachedProperties[PropertyName];
                else
                    cachedProperty = PropertyValue;

                IsCacheSynchronous = PropertyValueEqualityComparer.Equals(CachedPropertyValue, cachedProperty);
            }
        }

        private void SettingsPropertyNotifier_Changed(object sender, PropertyChangedEventArgs e)
            => recalculateIsCopySynchronous();

        private void SettingsPropertyCollectionNotifier_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => recalculateIsCopySynchronous();

        private void propertyChangedCache_PropertyCacheRemoved(PropertyType settingsProperty)
        {
            if (settingsProperty is INotifyPropertyChanged settingsPropertyNotifier)
                settingsPropertyNotifier.PropertyChanged -= SettingsPropertyNotifier_Changed;

            if (settingsProperty is INotifyCollectionChanged settingsPropertyCollectionNotifier)
                settingsPropertyCollectionNotifier.CollectionChanged -= SettingsPropertyCollectionNotifier_CollectionChanged;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void PropertyChangedCache_PropertyCacheAdded(object sender, PropertyCacheAddedEventArgs<PropertyType> args)
        {
            // When it is a recache, then first we have to remove the attached handlers we added before
            if (args.IsRecache)
                propertyChangedCache_PropertyCacheRemoved(args.RemovedProperty);

            Debug.WriteLine($"Property '{PropertyName}' has been added to the cache");

            var settingsProperty = args.AddedProperty;

            if (settingsProperty is INotifyPropertyChanged settingsPropertyNotifier)
                settingsPropertyNotifier.PropertyChanged += SettingsPropertyNotifier_Changed;

            if (settingsProperty is INotifyCollectionChanged settingsPropertyCollectionNotifier)
                settingsPropertyCollectionNotifier.CollectionChanged += SettingsPropertyCollectionNotifier_CollectionChanged;

            recalculateIsCopySynchronous();
            OnPropertyChanged(nameof(PropertyValue));
        }

        private void PropertyChangedCache_PropertyCacheRemoved(object sender, PropertyCacheRemovedEventArgs<PropertyType> args)
        {
            propertyChangedCache_PropertyCacheRemoved(args.Property);
            recalculateIsCopySynchronous();
        }

        private PropertyType copyValue(object propertyValue)
        {
            var serializer = new XmlSerializer(property.PropertyType);

            using (var stream = new MemoryStream())
            {
                using (var writer = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    serializer.Serialize(writer, propertyValue);
                    stream.Position = 0;
                    var copiedValue = (PropertyType)serializer.Deserialize(stream);
                    return copiedValue;
                }
            }
        }

        private PropertyType copySettingsPropertyDefaultValue()
        {
            var serializer = new XmlSerializer(property.PropertyType);
            object defaultPropertyValue = property.DefaultValue;
            bool isDefaultValueCopied = false;

            if (property.PropertyType.IsValueType && !property.PropertyType.IsEnum)
            {
                defaultPropertyValue = Convert.ChangeType(property.DefaultValue, property.PropertyType);
                isDefaultValueCopied = true;
            }
            else if (property.PropertyType == typeof(string))
            {
                defaultPropertyValue = property.DefaultValue.ToString();
                isDefaultValueCopied = true;
            }
            else if (defaultPropertyValue is string propertyString)
            {
                var bytes = Encoding.UTF8.GetBytes(propertyString);

                using (var stream = new MemoryStream(bytes))
                using (var reader = XmlReader.Create(stream))
                {
                    if (serializer.CanDeserialize(reader))
                    {
                        defaultPropertyValue = serializer.Deserialize(reader);
                        isDefaultValueCopied = true;
                    }
                }
            }

            PropertyType typedDefaultPropertyValue;

            if (isDefaultValueCopied)
                typedDefaultPropertyValue = (PropertyType)defaultPropertyValue;
            else
                typedDefaultPropertyValue = copyValue(defaultPropertyValue);

            return typedDefaultPropertyValue;
        }

        private PropertyType copySettingsPropertyValue()
        {
            var value = propertyValue;
            value = copyValue(value);
            return (PropertyType)value;
        }

        public virtual void RefreshCache(bool useDefaultValue)
        {
            PropertyType propertyValue;

            if (useDefaultValue)
                propertyValue = copySettingsPropertyDefaultValue();
            else
                propertyValue = copySettingsPropertyValue();

            CachedPropertyValue = propertyValue;
        }

        /// <summary>
        /// Notifies <see cref="settings"/> about changes from settings property "<see cref="PropertyName"/>".
        /// </summary>
        public void TriggerSettingsPropertyChanged()
        {
            if (NotifySettingsAboutPropertyChange != null)
                NotifySettingsAboutPropertyChange?.Invoke(PropertyName);
            else
                propertyValue = propertyValue;
        }

        /// <summary>
        /// It removes property value from cache and notifies <see cref="settings"/> about changes from settings property "<see cref="PropertyName"/>".
        /// </summary>
        public void SetOriginalFromDisk()
        {
            settings.PropertyValues.Remove(PropertyName);
            TriggerSettingsPropertyChanged();
        }

        private void setOriginalFromSource(object source)
        {
            propertyValue = source;
            TriggerSettingsPropertyChanged();
        }

        public void SetOriginalFromCache()
        {
            var copiedCopy = copyValue(CachedPropertyValue);
            setOriginalFromSource(copiedCopy);
        }

        public void SetOriginalFromDefault()
        {
            var defaultValue = copySettingsPropertyDefaultValue();
            setOriginalFromSource(defaultValue);
        }

        /// <summary>
        /// Reminder: <see cref="CachedPropertyValue"/> gets renewed by calling <see cref="SaveToDisk"/>.
        /// </summary>
        public void SaveToDisk()
        {
            settings.Save();
            RefreshCache(false);
            TriggerSettingsPropertyChanged();
        }
    }
}
