﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    partial class IJSObjectReferenceUtils
    {
        private static MethodInfo genericValueTaskReturningInvokeWithTimeSpanFunctionType = null!;
        private static MethodInfo valueTaskReturningInvokeWithTimeSpanFunctionType = null!;

        private static Dictionary<Type, Func<IJSObjectReference, string, TimeSpan, object?[], object>> invocableWithTimeSpanFunctions = null!;

        private static void InitializePartInvocableWithTimeSpan()
        {
            invocableWithTimeSpanFunctions = new Dictionary<Type, Func<IJSObjectReference, string, TimeSpan, object?[], object>>();

            genericValueTaskReturningInvokeFunction =
                jsObjectReferenceExtensionsType.GetMethod(
                    nameof(JSObjectReferenceExtensions.InvokeAsync),
                    genericParameterCount: 1,
                    types: new[] {
                        typeof(IJSObjectReference),
                        typeof(string),
                        typeof(TimeSpan),
                        typeof(object?[])
                    })
                ?? throw new InvalidOperationException();

            valueTaskReturningInvokeFunction =
                jsObjectReferenceExtensionsType.GetMethod(
                    nameof(JSObjectReferenceExtensions.InvokeAsync),
                    types: new[] {
                        typeof(IJSObjectReference),
                        typeof(string),
                        typeof(TimeSpan),
                        typeof(object?[])
                    })
                ?? throw new InvalidOperationException();
        }

        public static Func<IJSObjectReference, string, TimeSpan, object?[], object> GetInvocableWithTimeSpanFunction(ValueTaskType valueTaskType)
        {
            if (invocableWithTimeSpanFunctions.TryGetValue(valueTaskType.Type, out var invocableFunction)) {
                return invocableFunction;
            }

            var invokeFunction = valueTaskType.HasGenericParameterType
                ? CreateInvoker<Func<IJSObjectReference, string, TimeSpan, object?[], object>>(genericValueTaskReturningInvokeWithTimeSpanFunctionType)
                : CreateInvoker<Func<IJSObjectReference, string, TimeSpan, object?[], object>>(valueTaskReturningInvokeWithTimeSpanFunctionType);

            invocableWithTimeSpanFunctions.Add(valueTaskType.Type, invokeFunction);
            return invokeFunction;
        }
    }
}