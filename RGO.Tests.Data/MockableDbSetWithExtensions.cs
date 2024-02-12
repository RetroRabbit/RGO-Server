﻿using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace RGO.Tests.Data
{
    public abstract class MockableDbSetWithExtensions<T> : DbSet<T>
        where T : class
    {
        public abstract void AddOrUpdate(params T[] entities);
        public abstract void AddOrUpdate(Expression<Func<T, object>> identifierExpression, params T[] entities);
    }
}
