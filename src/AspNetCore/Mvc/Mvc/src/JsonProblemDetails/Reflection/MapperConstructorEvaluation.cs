﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.Reflection
{
    public class MapperConstructorEvaluation
    {
        public ConstructorInfo SourceInfo { get; }
        public MapperConstructorArea MapperConstructorArea { get; }
        public IReadOnlyCollection<ParameterEvaluation> ParameterEvaluations { get; }
        public ParameterEvaluation? FirstParameterEvaluation => ParameterEvaluations.FirstOrDefault();

        public MapperConstructorEvaluation(MapperConstructorArea area, ConstructorInfo sourceInfo,
            IReadOnlyCollection<ParameterEvaluation> parameterInfos)
        {
            MapperConstructorArea = area;
            SourceInfo = sourceInfo ?? throw new ArgumentNullException(nameof(sourceInfo));
            ParameterEvaluations = parameterInfos ?? throw new ArgumentNullException(nameof(parameterInfos));
        }
    }
}
