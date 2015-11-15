﻿using Saule.Queries.Pagination;
using Saule.Queries.Sorting;

namespace Saule.Queries
{
    internal class QueryContext
    {
        public PaginationContext Pagination { get; set; }

        public SortingContext Sorting { get; set; }
    }
}
