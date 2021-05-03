using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Wrappers
{
    public class PostResponse<T>
    {
        public int? SubTopicId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string SortOrder { get; set; }
        public string SortType { get; set; }
        public T Data { get; set; }

        public PostResponse(T data, int? subTopicId, int page, int size, int count, string order, string type)
        {
            SubTopicId = subTopicId;
            PageNumber = page;
            PageSize = size;
            TotalPages = GetPages(count, size);
            TotalRecords = count;
            SortOrder = GetOrder(order);
            SortType = GetType(type);
            Data = data;
        }

        static int GetPages(int count, int size)
        {
            int result = (count / size);
            if (count % size != 0) { result++; }
            return result;
        }

        static string GetOrder(string order)
        {
            if (order == "Asc")
            {
                return "Ascending";
            }
            else
            {
                return "Descending";
            }
        }

        static string GetType(string type)
        {
            if (type == "Comment")
            {
                return "Comment_Count";
            }
            else if (type == "Like")
            {
                return "Like_Count";
            }
            else
            {
                return "Date";
            }
        }
    }
}
