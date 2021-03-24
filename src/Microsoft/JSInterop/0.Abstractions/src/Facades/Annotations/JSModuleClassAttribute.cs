﻿using System;

namespace Teronis.Microsoft.JSInterop.Facades.Annotations
{
    /// <summary>
    /// Decoratable on class. It provides
    /// <see cref="JSModuleAttributeBase.ModuleNameOrPath"/>
    /// to those properties with facade attribute that are using
    /// parameterless constructor but not
    /// <see cref="JSModulePropertyAttribute(string)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class JSModuleClassAttribute : JSModuleAttributeBase
    {
        public JSModuleClassAttribute(string ModuleNameOrPath)
            : base(ModuleNameOrPath) { }
    }
}