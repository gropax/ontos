using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scenes.Web.Contracts
{
    public class PaginationParamsDto<TColumn> where TColumn : struct
    {
        public int Page { get; }
        public int PageSize { get; }
        public string SortColumn { get; }
        public string SortDirection { get; }

        public PaginationParamsDto(int page, int pageSize, string sortColumn, string sortDirection)
        {
            Page = page;
            PageSize = pageSize;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }

        public PaginationParams<TColumn> ToModel()
        {
            return new PaginationParams<TColumn>(
                Page, PageSize,
                Enum.Parse<TColumn>(SortColumn, ignoreCase: true),
                Enum.Parse<SortDirection>(SortDirection, ignoreCase: true));
        }
    }
}
