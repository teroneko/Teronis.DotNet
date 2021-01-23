﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Teronis.Mvc.JsonProblemDetails.Mappers.Description;
using Teronis.Mvc.JsonProblemDetails.Filters;
using Teronis.Mvc.JsonProblemDetails.Mappers;
using Teronis.Mvc.JsonProblemDetails.MappableObjectResolvers;

namespace Teronis.Mvc.JsonProblemDetails
{
    public class ProblemDetailsOptions
    {
        /// <summary>
        /// Collection of <see cref="ProblemDetailsMapperDescriptor"/> which are used to describe
        /// suitable mappers.
        /// </summary>
        public ProblemDetailsMapperDescriptorCollection MapperDescriptors { get; }
        /// <summary>
        /// Order of execution of <see cref="ProblemDetailsActionFilter"/>. 
        /// Default is the <see cref="int.MaxValue"/> - 10 (magic number).
        /// </summary>
        public int ActionResultFilterOrder { get; set; }
        public int ExceptionFilterOrder { get; set; }
        /// <summary>
        /// Collection of <see cref="IActionResult"/>s object resolver which enables you to find
        /// mappable objects.
        /// </summary>
        public List<IMappableObjectResolver> MappableObjectResolvers { get; }

        public ProblemDetailsOptions()
        {
            MapperDescriptors = new ProblemDetailsMapperDescriptorCollection();
            var filterOrder = int.MaxValue - 10;
            ActionResultFilterOrder = filterOrder;
            ExceptionFilterOrder = filterOrder;
            MappableObjectResolvers = new List<IMappableObjectResolver>();
        }

        public class ProblemDetailsMapperDescriptorCollection : IReadOnlyCollection<ProblemDetailsMapperDescriptor>
        {
            public int Count => mapperDescriptors.Count;

            private readonly List<ProblemDetailsMapperDescriptor> mapperDescriptors;

            public ProblemDetailsMapperDescriptorCollection() =>
                mapperDescriptors = new List<ProblemDetailsMapperDescriptor>();

            public void RemoveAt(int index) =>
                mapperDescriptors.RemoveAt(index);

            public IEnumerator<ProblemDetailsMapperDescriptor> GetEnumerator() =>
                mapperDescriptors.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() =>
                (mapperDescriptors as IEnumerable).GetEnumerator();

            private void describeExceptionProblemDetailsMapper(Type mapperType, ProblemDetailsMapperDescriptorOptions? options, 
                Action<ProblemDetailsMapperDescriptor> processDescriptor)
            {
                var allowDerivedMapperSourceObjectTypes = options?.AllowDerivedMappableObjectTypes ?? false;
                var statusCodes = options?.StatusCodes;
                mapperType = mapperType ?? throw new ArgumentNullException(nameof(mapperType));
                var descriptor = new ProblemDetailsMapperDescriptor(mapperType, allowDerivedMapperSourceObjectTypes, statusCodes);
                processDescriptor(descriptor);
            }

            public void Add(Type mapperType, ProblemDetailsMapperDescriptorOptions? options = null) =>
                describeExceptionProblemDetailsMapper(mapperType, options, descriptor => mapperDescriptors.Add(descriptor));

            public void Add<MapperType>(ProblemDetailsMapperDescriptorOptions? options = null)
                where MapperType : class, IProblemDetailsMapper =>
                Add(typeof(MapperType), options);

            public void Insert(Type mapperType, int index, ProblemDetailsMapperDescriptorOptions? options = null) =>
                describeExceptionProblemDetailsMapper(mapperType, options, descriptor => mapperDescriptors.Insert(index, descriptor));

            public void Insert<MapperType>(int index, ProblemDetailsMapperDescriptorOptions? options = null)
                where MapperType : class, IProblemDetailsMapper =>
                Insert(typeof(MapperType), index, options);

            public bool TryFind(int startIndex, int? statusCode, [NotNullWhen(true)] out (ProblemDetailsMapperDescriptor MapperDescriptor, int NextIndex)? find)
            {
                var mapperDescriptors = this.mapperDescriptors;

                for (var index = startIndex; index < mapperDescriptors.Count; index++) {
                    var mapperDescriptor = mapperDescriptors[index];

                    if (mapperDescriptor.IsStatusCodeSuitable(statusCode)) {
                        find = (mapperDescriptor, index + 1);
                        return true;
                    } 
                }

                find = null;
                return false;
            }
        }
    }
}
