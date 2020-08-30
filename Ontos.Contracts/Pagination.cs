using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ontos.Contracts
{
    public class PaginationParams<TColumn>
    {
        public int Page { get; }
        public int PageSize { get; }
        public TColumn SortColumn { get; }
        public SortDirection SortDirection { get; }
        public int Skip => (Page - 1) * PageSize;

        public PaginationParams(int page, int pageSize, TColumn sortColumn, SortDirection sortDirection)
        {
            Page = page;
            PageSize = pageSize;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }
    }

    public class Paginated<TItem>
    {
        public int Page { get; }
        public int PageSize { get; }
        public string SortColumn { get; }
        public SortDirection SortDirection { get; }
        public long Total { get; }
        public TItem[] Items { get; }

        public Paginated(int page, int pageSize, string sortColumn, SortDirection sortDirection, long total, TItem[] items)
        {
            Page = page;
            PageSize = pageSize;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
            Total = total;
            Items = items;
        }

        public static Paginated<TItem> FromParams<TColumn>(PaginationParams<TColumn> p, long total, TItem[] items)
        {
            return new Paginated<TItem>(
                p.Page,
                p.PageSize,
                p.SortColumn.ToString(),
                p.SortDirection,
                total,
                items);
        }
    }


    public enum SortDirection
    {
        ASC,
        DESC,
    }


    public static class PaginatedExtensions
    {
        public static Paginated<U> Map<T, U>(this Paginated<T> p, Func<T, U> map)
        {
            return new Paginated<U>(p.Page, p.PageSize, p.SortColumn, p.SortDirection, p.Total,
                p.Items.Select(i => map(i)).ToArray());
        }
    }
}
