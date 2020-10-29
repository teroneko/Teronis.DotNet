﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Teronis.Data;

namespace Teronis.Utils
{
    public static class TeronisUtils
    {
        public static bool CompareEquality<T>([AllowNull] T one, [AllowNull] T two) =>
            EqualityComparer<T>.Default.Equals(one!, two!);

        public static bool ReturnNonDefault<T>(T inValue, [MaybeNull] out T outValue, Func<T>? getNonDefaultWhenDefault = null)
        //=> !CompareEquality(outValue = inValue, default) || (FuncGenericUtils.ReturnIsInvocable(getNonDefaultIfDefault, out outValue) && !CompareEquality(outValue, default));
        {
            outValue = inValue;
            var isValueNotEqualsDefault = !CompareEquality(outValue, default);

            if (isValueNotEqualsDefault) {
                return true;
            } else {
                if (FuncGenericUtils.IsInvocable(getNonDefaultWhenDefault, out outValue)) {
                    return !CompareEquality(outValue, default);
                } else {
                    return false;
                }
            }
        }

        public static I ReturnInValue<I>(I inValue) =>
            inValue;

        public static I ReturnInValue<I>(I inValue, out I outInValue)
        {
            outInValue = inValue;
            return inValue;
        }

        public static I ReturnInValue<I>(I inValue, Action<I> mutateInValue)
        {
            mutateInValue(inValue);
            return inValue;
        }

        public static I ReturnInValue<I>(I inValue, Func<I, I> modifyInValue)
            => modifyInValue(inValue);

        public static I ReturnInValue<I>(I inValue, Action doSomething)
        {
            doSomething();
            return inValue;
        }

        public static async Task<I> ReturnInValue<I>(I inValue, Task task)
        {
            await task;
            return inValue!;
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, V returnBoolean)
        {
            outInValue = inValue;
            return returnBoolean;
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, Func<V> getValue)
        {
            outInValue = inValue;
            return getValue();
        }

        [return: MaybeNull]
        public static V ReturnValue<I, V>(I inValue, out I outInValue, Func<I, V> getValue)
        {
            outInValue = inValue;
            return getValue(inValue);
        }

        public static V ReturnValue<I, V>(I inValue, Func<I, V> getValue)
            => getValue(inValue);

        public static V ReturnValue<I, V>(I inValue, Func<V> getValue)
            => getValue();

        /// <summary>
        /// Useful for unsubscribing inline event handlers.
        /// </summary>
        public static WrappedValue<T> ReturnDefaultReplacement<T>(Func<WrappedValue<T>, T> getDefaultValueReplacement)
        {
            var defaultValueWrapper = new WrappedValue<T>(default);
            defaultValueWrapper.Value = getDefaultValueReplacement(defaultValueWrapper);
            return defaultValueWrapper;
        }

        [return: MaybeNull]
        public static T ReturnDefaultValueReplacement<T>(Func<WrappedValue<T>, T> getDefaultValueReplacement) =>
            ReturnDefaultReplacement(getDefaultValueReplacement).Value;
    }
}
