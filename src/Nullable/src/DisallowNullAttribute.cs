﻿// <auto-generated>
//   This code file has automatically been added by the "Nullable" NuGet package (https://www.nuget.org/packages/Nullable).
//   Please see https://github.com/manuelroemer/Nullable for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Nullable" folder and the nullable "*Attribute.cs" files don't appear in your project.
//   * The added files are immutable and can therefore not be modified by coincidence.
//   * Updating/Uninstalling the package will work flawlessly.
// </auto-generated>

#region License
// MIT License
// 
// Copyright (c) Manuel Römer
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

#if !NULLABLE_ATTRIBUTES_DISABLE
#nullable enable
#pragma warning disable

namespace System.Diagnostics.CodeAnalysis
{
    using global::System;

#if DEBUG
    /// <summary>
    ///     Specifies that <see langword="null"/> is disallowed as an input even if the
    ///     corresponding type allows it.
    /// </summary>
#endif
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property,
        Inherited = false
    )]
#if !NULLABLE_ATTRIBUTES_INCLUDE_IN_CODE_COVERAGE
    [DebuggerNonUserCode]
#endif
    internal sealed partial class DisallowNullAttribute : Attribute
    {
#if DEBUG
        /// <summary>
        ///     Initializes a new instance of the <see cref="DisallowNullAttribute"/> class.
        /// </summary>
#endif
        public DisallowNullAttribute() { }
    }
}

#pragma warning restore
#nullable restore
#endif // NULLABLE_ATTRIBUTES_DISABLE
