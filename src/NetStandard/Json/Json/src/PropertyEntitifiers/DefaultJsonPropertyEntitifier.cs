﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teronis.Json.PropertyEntitifiers
{
    public class DefaultJsonPropertyEntitifier<ValueType> : IJsonPropertyEntitifiable<ValueType>
    {
        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="serializer"></param>
        /// <returns>Expect a value.</returns>
        public ValueType EntitifyJsonProperty(JProperty property, JsonSerializer serializer) =>
            property.Value.ToObject<ValueType>(serializer);
    }
}
