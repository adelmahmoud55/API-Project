using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Entities.Products
{
    public class ProductBrand : BaseEntity<int>
    {
        public required string Name { get; set; }
    }

    #region init Vs set
    //The init accessor allows you to have a default value while still enabling one-time changes through object initializers, providing flexibility in how you configure your objects.
    //However, after the object is created, the property becomes immutable, preserving its integrity.


    #endregion
}
