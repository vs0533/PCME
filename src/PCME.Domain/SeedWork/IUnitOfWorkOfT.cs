﻿// Copyright (c) Arch team. All rights reserved.

using Microsoft.EntityFrameworkCore;
using PCME.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Domain.SeedWork {
    /// <summary>
    /// Defines the interface(s) for generic unit of work.
    /// </summary>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext {
        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
        TContext DbContext { get; }

        /// <summary>
        /// Saves all changes made in this context to the database with distributed transaction.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks);

        Task<bool> SaveEntitiesAsync(bool ensureAutoHistory = false, CancellationToken cancellationToken = default(CancellationToken),params IUnitOfWork[] unitOfWorks);
    }
}
