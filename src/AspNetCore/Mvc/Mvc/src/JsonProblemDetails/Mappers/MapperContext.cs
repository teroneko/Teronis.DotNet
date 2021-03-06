﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers
{
    public static class MapperContext
    {
        internal static object CreateFromObject(object mappableObject, int? statusCode, ProblemDetailsFactoryScoped problemDetailsFactory)
        {
            mappableObject = mappableObject ?? throw new ArgumentNullException(nameof(mappableObject));
            var mappableObjectType = mappableObject.GetType();
            var mapperContextType = typeof(MapperContext<>).MakeGenericType(mappableObjectType);

            return Activator.CreateInstance(mapperContextType, mappableObject, statusCode, problemDetailsFactory)
                ?? throw new InvalidOperationException("No constructor has been found.");
        }
    }
}
