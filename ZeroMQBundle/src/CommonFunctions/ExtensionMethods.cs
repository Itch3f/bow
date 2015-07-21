using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFunctions
{
  public static  class ExtensionMethods
    {
      public static IEnumerable<U> ForEach<T, U>(this IEnumerable<T> enumeration, Func<T, U> func)
      {
          foreach (T item in enumeration)
          {
              yield return func(item);
          }       
      }
    }
}
