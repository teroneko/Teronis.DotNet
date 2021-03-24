﻿using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public sealed class ComponentProperty : MemberInfoAttributeLookup, IComponentProperty
    {
        public PropertyInfo PropertyInfo =>
            propertyInfo;

        public Type PropertyType =>
            PropertyInfo.PropertyType;

        public ComponentPropertyType ComponentPropertyType {
            get {
                if (propertyType is null) {
                    propertyType = new ComponentPropertyType(propertyInfo.PropertyType);
                }

                return propertyType;
            }
        }

        private readonly PropertyInfo propertyInfo;
        private ComponentPropertyType? propertyType;

        public ComponentProperty(PropertyInfo propertyInfo)
            : base(propertyInfo) =>
            this.propertyInfo = propertyInfo;

        IComponentPropertyType IComponentProperty.ComponentPropertyType =>
            ComponentPropertyType;
    }
}