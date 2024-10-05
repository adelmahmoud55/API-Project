using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Comman
{
    public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey> // When you compare objects in C#, the default equality comparison uses reference equality (i.e., it checks if both references point to the same object in memory). By implementing IEquatable<T>, you can define how instances of your entity should be compared for equality based on their value (like their ID).

    {
        public required TKey Id { get; set; }  

        public required string CreatedBy { get; set; }

        public  DateTime CreatedOn { get; set; } /*= DateTime.UtcNow;*/

        public required string LastModifiedBy { get; set; }

        public DateTime LastModifiedOn { get; set; } /*= DateTime.UtcNow;*/
    }


    #region required KeyWoard
    // required is a new C# 8.0 feature that allows you to specify that a property or field must be set in a constructor.
    // This is useful for immutable objects, where you want to ensure that all properties are set when the object is created.
    // it prevents new from initializing the specified properties to their default values. {null}


    #endregion
}