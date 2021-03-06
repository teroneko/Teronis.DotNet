﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Teronis.Collections.Specialized;

namespace Teronis.Microsoft.JSInterop.Dynamic.Reflection
{
    internal class MethodDictionary
    {
        public int Count =>
            methodByMethodNameDictionary.Count;

        private LinkedBucketList<string, Method> methodByMethodNameDictionary;

        public MethodDictionary()
        {
            methodByMethodNameDictionary = new LinkedBucketList<string, Method>();
        }

        public void AddMethod(MethodInfo methodInfo, ParameterList parameterList, ValueTaskType valueTaskType) =>
            methodByMethodNameDictionary.Add(methodInfo.Name, new Method(methodInfo, parameterList, valueTaskType));

        public bool TryFindMethod(string name, IEnumerable<string?> argumentNames, [MaybeNullWhen(false)] out Method method)
        {
            if (!methodByMethodNameDictionary.TryGetBucket(name, out var methodBucket)) {
                method = null;
                return false;
            }

            method = methodBucket.FindFirst(x =>
                x.ParameterList.ArgumentNames.SequenceEqual(argumentNames, ArgumentNameEqualityComparer.Default))?.Value;

            return method != null;
        }

        private class ArgumentNameEqualityComparer : EqualityComparer<string?>
        {
            public new static ArgumentNameEqualityComparer Default => new ArgumentNameEqualityComparer();

            public override bool Equals(string? parameterInfoName, string? positionalArgumentName)
            {
                if (positionalArgumentName == null) {
                    return true;
                }

                return parameterInfoName == positionalArgumentName;
            }

            public override int GetHashCode([DisallowNull] string obj) =>
                obj.GetHashCode();
        }
    }
}
