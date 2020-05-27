﻿using Microsoft.EntityFrameworkCore;
using Teronis.Identity.Entities;

namespace Teronis.Identity
{
    public class BearerIdentityDbContext : BearerIdentityDbContext<UserEntity, RoleEntity>
    {
        public BearerIdentityDbContext()
            : base() { }

        public BearerIdentityDbContext(DbContextOptions options)
            : base(options) { }
    }
}
