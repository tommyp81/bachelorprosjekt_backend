using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Wrappers
{
    public class PageResponse<T>
    {
        public int? Id { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string SortOrder { get; set; }
        public string SortType { get; set; }
        public T Data { get; set; }

        public PageResponse(T data)
        {
            Id = null;
            PageNumber = 1;
            PageSize = 10;
            TotalPages = 0;
            TotalRecords = 0;
            SortOrder = "Asc";
            SortType = "Id";
            Data = data;
        }
    }
}
