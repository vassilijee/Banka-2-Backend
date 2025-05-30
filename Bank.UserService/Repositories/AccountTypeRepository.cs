﻿using System.Linq.Expressions;

using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Bank.UserService.Repositories;

public interface IAccountTypeRepository
{
    Task<Page<AccountType>> FindAll(AccountTypeFilterQuery accountTypeFilterQuery, Pageable pageable);

    Task<AccountType?> FindById(Guid id);

    Task<AccountType> Add(AccountType accountType);

    Task<AccountType> Update(AccountType accountType);
}

public class AccountTypeRepository(ApplicationContext context) : IAccountTypeRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<Page<AccountType>> FindAll(AccountTypeFilterQuery accountTypeFilterQuery, Pageable pageable)
    {
        var accountTypeQuery = m_Context.AccountTypes.AsQueryable();

        if (!string.IsNullOrEmpty(accountTypeFilterQuery.Name))
            accountTypeQuery = accountTypeQuery.Where(accountType => EF.Functions.ILike(accountType.Name, $"%{accountTypeFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(accountTypeFilterQuery.Name))
            accountTypeQuery = accountTypeQuery.Where(accountType => EF.Functions.ILike(accountType.Code, $"%{accountTypeFilterQuery.Code}%"));

        var accountTypes = await accountTypeQuery.Skip((pageable.Page - 1) * pageable.Size)
                                                 .Take(pageable.Size)
                                                 .ToListAsync();

        var totalElements = await accountTypeQuery.CountAsync();

        return new Page<AccountType>(accountTypes, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<AccountType?> FindById(Guid id)
    {
        return await m_Context.AccountTypes.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<AccountType> Add(AccountType accountType)
    {
        var addedAccountType = await m_Context.AccountTypes.AddAsync(accountType);

        await m_Context.SaveChangesAsync();

        return addedAccountType.Entity;
    }

    public async Task<AccountType> Update(AccountType accountType)
    {
        await m_Context.AccountTypes.Where(dbAccountType => dbAccountType.Id == accountType.Id)
                       .ExecuteUpdateAsync(setters => setters.SetProperty(dbAccountType => dbAccountType.Name, accountType.Name)
                                                             .SetProperty(dbAccountType => dbAccountType.Code,       accountType.Code)
                                                             .SetProperty(dbAccountType => dbAccountType.ModifiedAt, accountType.ModifiedAt));

        return accountType;
    }
}

public static partial class RepositoryExtensions
{
    [Obsolete("This method does not have implementation.", true)]
    public static IIncludableQueryable<AccountType, object?> IncludeAll(this DbSet<AccountType> set)
    {
        return set.Include(accountType => accountType);
    }

    public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, AccountType?> value,
                                                                                 Expression<Func<TEntity, AccountType?>> navigationExpression, params string[] excludeProperties)
    where TEntity : class
    {
        return value;
    }

    public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, List<AccountType>> value,
                                                                                 Expression<Func<TEntity, List<AccountType>>>          navigationExpression,
                                                                                 params string[]                                       excludeProperties) where TEntity : class
    {
        return value;
    }
}
