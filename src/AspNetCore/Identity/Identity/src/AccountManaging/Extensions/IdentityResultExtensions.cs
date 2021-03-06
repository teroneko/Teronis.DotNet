﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Teronis.AspNetCore.Identity.AccountManaging.Extensions
{
    internal static class IdentityResultExtensions
    {
        private static IEnumerable<IdentityError>? getIdentityResultErrorsOrThrow(IdentityResult? identityResult)
        {
            identityResult = identityResult ?? throw new ArgumentNullException(nameof(identityResult));

            if (identityResult.Succeeded) {
                throw new ArgumentException("There are no errors because the result is successful.");
            }

            return identityResult.Errors;
        }

        public static AggregateException ToAggregateException(this IdentityResult identityResult, string? errorMessage = null)
        {
            var identityErrors = getIdentityResultErrorsOrThrow(identityResult);

            return new AggregateException(errorMessage, from error in identityResult.Errors
                                                        select new Exception($"{error.Description} ({error.Code})"));
        }

        public static KeyedAggregateException ToKeyAggregateException(this IdentityResult identityResult, string? errorMessage = null)
        {
            var identityErrors = getIdentityResultErrorsOrThrow(identityResult);

            var keyedExceptions = identityResult.Errors.ToDictionary(
                x => x.Code,
                x => new Exception(x.Description));

            return new KeyedAggregateException(errorMessage, keyedExceptions);
        }
    }
}
