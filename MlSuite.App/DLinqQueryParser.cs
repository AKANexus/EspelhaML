using MlSuite.Domain.Entities;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace MlSuite.App
{
    public static class DLinqQueryParser
    {
        public static IQueryable<TEntity>? ApplyFiltering<TEntity>(this IQueryable<TEntity> dbQuery, FilteredQuery filtering)
        {
            foreach (Filter queryFilter in filtering.Filters)
            {
                string property = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(queryFilter.Field);
                switch (queryFilter.Method)
                {
                    case FilterType.ComecaCom:
                        dbQuery = dbQuery.Where($"{property}.StartsWith(\"{queryFilter.Query}\")");
                        break;
                    case FilterType.Contem:
                        dbQuery = dbQuery.Where($"{property}.Contains(\"{queryFilter.Query}\")");
                        break;
                    case FilterType.Igual:
                        dbQuery = dbQuery.Where($"{property} == \"{queryFilter.Query}\")");
                        break;
                    case FilterType.Diferente:
                        dbQuery = dbQuery.Where($"{property} != \"{queryFilter.Query}\")");
                        break;
                    case FilterType.MaiorQue:
                        dbQuery = dbQuery.Where($"{property} < \"{queryFilter.Query}\")");
                        break;
                    case FilterType.MenorQue:
                        dbQuery = dbQuery.Where($"{property} > \"{queryFilter.Query}\")");
                        break;
                    default:
                        return null;
                }
            }

            return dbQuery;
        }

    }
}
