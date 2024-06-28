using Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectPoolManager
{
  public enum EObjectPoolType
  { 
   UI,
   Debug
  }
  
  class ObjectPool
  {
    public Transform Transform;
    public string PrefabName;
    public List<GameObject> Objects = new List<GameObject>();

    public ObjectPool(Transform transform, string prefabName)
    {
      Transform = transform;
      PrefabName = prefabName;
    }
  }

  private static Dictionary<EObjectPoolType, ObjectPool> _pools = new Dictionary<EObjectPoolType, ObjectPool>();

  // ===================================================================================================
  public static void AddPoll(EObjectPoolType type, Transform transform, string prefabName)
  {
    _pools.Add(type, new ObjectPool(transform, prefabName));
  }

  // ===================================================================================================
  public static GameObject Spawn(EObjectPoolType type)
  {
    if (!_pools.ContainsKey(type))
      return null;

    var pool = _pools[type];

    var go = pool.Objects.FirstOrDefault(o => !o.activeSelf);

    if (go == null)
    {
      go = Factory.Instance.GetObjectPool(pool.PrefabName);
      pool.Objects.Add(go);
    }

    go.transform.SetParentWithParam(pool.Transform);

    return go;
  }

  // ===================================================================================================
  public static void DeSpawn(GameObject go)
  {
    go.SetActive(false);
  }

}
