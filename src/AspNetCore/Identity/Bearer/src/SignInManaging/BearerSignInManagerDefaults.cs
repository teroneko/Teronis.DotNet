﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.AspNetCore.Identity.Bearer.SignInManaging;

namespace Teronis.AspNetCore.Identity.Bearer
{
    public static class BearerSignInManagerDefaults
    {
        /// <summary>
        /// Used to find bearer refresh token in claims of identity.
        /// </summary>
        public const string BearerSignInManagerRefreshTokenIdClaimType = nameof(BearerSignInManager) + "." +  nameof(SignInTokens.RefreshToken) + "Id";
    }
}
