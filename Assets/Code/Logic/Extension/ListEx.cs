using Static;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Extension
{
  public static class ListEx
  {
    // ===================================================================================================
    public static T GetRandomElement<T>(this List<T> list)
    {
      if ((list == null) || !list.Any())
        return default(T);

      return list[Random.Range(0, list.Count)];
    }

    // ===================================================================================================
    public static List<Consume> Clone(this List<Consume> list)
    {
      if ((list == null) || !list.Any())
        return new List<Consume>();

      List<Consume> clone = new List<Consume>();

      foreach (var item in list)
      {
        var newItem = new Consume(item.Type, item.Amount);
        clone.Add(newItem);
      }
      return clone;
    }
  }
}
