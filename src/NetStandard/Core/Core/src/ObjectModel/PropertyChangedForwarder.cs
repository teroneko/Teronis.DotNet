﻿using System;
using System.ComponentModel;

namespace Teronis.ObjectModel
{
    public class PropertyChangedForwarder : PropertyChangeForwarder<INotifyPropertyChanged, PropertyChangedEventArgs>, INotifyPropertyChanged
    {
        private static Action<object, PropertyChangedEventArgs>? convertNotifyPropertyChangeMethod(Action<PropertyChangedEventArgs>? notifyPropertyChanged)
        {
            if (notifyPropertyChanged is null) {
                return null;
            } else {
                return (sender, args) => notifyPropertyChanged(args);
            }
        }

        private static Action<object, PropertyChangedEventArgs>? convertNotifyPropertyChangeMethod(Action<string>? notifyPropertyChanged)
        {
            if (notifyPropertyChanged is null) {
                return null;
            } else {
                return (sender, args) => notifyPropertyChanged(args.PropertyName);
            }
        }

        /// <summary>
        /// Notifies about forwarded property changes from <see cref="PropertyContainer"/>.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChangedForward {
            add => EventInvocationForward += value!.Invoke;
            remove => EventInvocationForward -= value!.Invoke;
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add => PropertyChangedForward += value;
            remove => PropertyChangedForward -= value;
        }

        protected override Action<object, PropertyChangedEventArgs>? ForwardEventInvocation { get; }

        public PropertyChangedForwarder(INotifyPropertyChanged propertyContainer, Action<object, PropertyChangedEventArgs>? notifyPropertyChange = null, object? alternativeEventSender = null)
            : base(propertyContainer, alternativeEventSender: alternativeEventSender) =>
            ForwardEventInvocation = notifyPropertyChange;

        public PropertyChangedForwarder(INotifyPropertyChanged propertyContainer, object? alternativeEventSender = null)
           : this(propertyContainer, default(Action<object, PropertyChangedEventArgs>?), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangedForwarder(INotifyPropertyChanged propertyContainer)
           : this(propertyContainer, default(Action<object, PropertyChangedEventArgs>?), alternativeEventSender: default) { }

        public PropertyChangedForwarder(INotifyPropertyChanged propertyContainer, Action<PropertyChangedEventArgs>? notifyPropertyChange = null, object? alternativeEventSender = null)
            : this(propertyContainer, notifyPropertyChange: convertNotifyPropertyChangeMethod(notifyPropertyChange), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangedForwarder(INotifyPropertyChanged propertyContainer, Action<string>? notifyPropertyChange = null, object? alternativeEventSender = null)
            : this(propertyContainer, convertNotifyPropertyChangeMethod(notifyPropertyChange), alternativeEventSender: alternativeEventSender) { }

        protected override bool CanForwardEventInvocation(PropertyChangedEventArgs eventArgs) =>
            CalleePropertyNameByCallerPropertyNameDictionary.ContainsKey(eventArgs.PropertyName);

        protected override PropertyChangedEventArgs CreateEventArgument(PropertyChangedEventArgs eventArgs) {
            var calleePropertyName = CalleePropertyNameByCallerPropertyNameDictionary[eventArgs.PropertyName];
            var forwardingEventArgs = new PropertyChangedEventArgs(calleePropertyName);
            return forwardingEventArgs;
        }

        private void PropertyContainer_PropertyChanged(object sender, PropertyChangedEventArgs args) =>
            OnEventInvocationForward(sender, args);
    }
}
