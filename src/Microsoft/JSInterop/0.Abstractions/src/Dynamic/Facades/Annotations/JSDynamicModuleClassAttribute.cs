﻿using System;
using Teronis.Microsoft.JSInterop.Facades.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.Facades.Annotations
{
    /// <summary>
    /// Decoratable on interface. It provides
    /// <see cref="JSModuleAttributeBase.ModuleNameOrPath"/>
    /// to those properties with facade attribute that are using
    /// parameterless constructor but not
    /// <see cref="JSDynamicModulePropertyAttribute(string)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true)]
    public class JSDynamicModuleClassAttribute : JSModuleClassAttribute
    {
        public JSDynamicModuleClassAttribute(string ModuleNameOrPath)
            : base(ModuleNameOrPath) { }
    }
}