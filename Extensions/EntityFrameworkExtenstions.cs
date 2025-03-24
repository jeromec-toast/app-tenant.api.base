using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Tenant.API.Base.Extensions
{
    public static class EntityFrameworkExtenstions
    {
        /// <summary>
        /// Create transaction async
        /// </summary>
        /// <returns></returns>
        private static TransactionScope CreateTrancationAsync()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                                    new TransactionOptions()
                                    {
                                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                                    },
                                    TransactionScopeAsyncFlowOption.Enabled);
        }

        /// <summary>
        /// Create transaction
        /// </summary>
        /// <returns></returns>
        private static TransactionScope CreateTrancation()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                                    new TransactionOptions()
                                    {
                                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                                    });
        }

        /// <summary>
        /// ToList with nolock
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static List<T> ToListWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            List<T> result = default;
            using (var scope = CreateTrancation())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                result = query.ToList();
                scope.Complete();
            }
            return result;
        }

        /// <summary>
        /// ToList with nolock async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, Expression<Func<T, bool>> expression = null)
        {
            List<T> result = default;
            using (var scope = CreateTrancationAsync())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                result = await query.ToListAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        /// <summary>
        /// FirstOrDefault with nolock
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static T FirstOrDefaultWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTrancation())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                T result = query.FirstOrDefault();
                scope.Complete();
                return result;
            }
        }

        /// <summary>
        /// FirstOrDefault with nolock async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static async Task<T> FirstOrDefaultWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTrancationAsync())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                T result = await query.FirstOrDefaultAsync(cancellationToken);
                scope.Complete();
                return result;
            }
        }

        /// <summary>
        /// Count with nolock
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int CountWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTrancation())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                int toReturn = query.Count();
                scope.Complete();
                return toReturn;
            }
        }

        /// <summary>
        /// Count with nolock async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static async Task<int> CountWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTrancationAsync())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                int toReturn = await query.CountAsync(cancellationToken);
                scope.Complete();
                return toReturn;
            }
        }
    }
}
