using LinkDev.Talabat.Core.Application.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Comman
{
    public class Pagination<T>
    {
        public Pagination(int pageSize, int pageIndex,int count)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public int Count { get; set; }

        public required IEnumerable<T> Data { get; set; }
        
    }
}
