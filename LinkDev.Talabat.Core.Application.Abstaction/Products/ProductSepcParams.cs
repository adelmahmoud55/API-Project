using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Products
{
    // if we have end point that takes moew than 3  parameters, we can merge them in one class as per clean code
    public class ProductSepcParams
    {
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

        //pagination , default first page with 5 items
        public int PageIndex { get; set; } = 1; // default : first page

        private const int MaxPageSize = 10;

        private int pageSize = 5; // default : 5 items per page

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value ; }
        }

    }
}
