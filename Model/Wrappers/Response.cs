using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Wrappers
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
