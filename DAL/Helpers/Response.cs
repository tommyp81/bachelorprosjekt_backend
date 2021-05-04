using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class Response<T>
    {
        public int Count { get; set; }
        public T Data { get; set; }

        public Response(T data, int count)
        {
            Count = count;
            Data = data;
        }
    }
}
