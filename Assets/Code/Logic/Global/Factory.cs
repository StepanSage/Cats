using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Objects
{
  //=======================================================================================================================
  public enum EFactoryObjectType
  {
    Level,
    Building,
    Product,
    Icon,
    Npc,
    Dialog,
    Pool,
    Sound,
    PointLevel,
    Decor,
    Panel
  }

  //=======================================================================================================================
  [Serializable]
  public class FactoryObject
  {
    [SerializeField] public string id;
    [SerializeField] public GameObject prefab;
  }

  //=======================================================================================================================
  [Serializable]
  public class FactoryIcon
  {
    [SerializeField] public string id;
    [SerializeField] public Sprite sprite;
  }

  //=======================================================================================================================
  [Serializable]
  public class FactorySound
  {
    [SerializeField] public string Id;
    [SerializeField] public AudioClip Sound;
  }

  //=======================================================================================================================
  [CreateAssetMenu(fileName = "Factory", menuName = "ScriptableObject/Factory")]
  public class Factory : ScriptableObject
  {
    private static Factory m_factory = null;
    public static Factory Instance
    {
      get
      {
        if (m_factory == null)
        {
          m_factory = Resources.Load("Factory") as Factory;
          if (m_factory == null)
          {
            m_factory = CreateInstance<Factory>();
#if UNITY_EDITOR
            string fullPath = "Assets\\Resources\\Factory.asset";
            AssetDatabase.CreateAsset(m_factory, fullPath);
#endif
          }
          m_factory.Init();
        }
        return m_factory;
      }
    }

    //=======================================================================================================================
    [Header("Icons")]
    [SerializeField]
    private List<FactoryIcon> _productIcons = new List<FactoryIcon>();
    [SerializeField]
    private List<FactoryIcon> _mainIcons = new List<FactoryIcon>();
    [SerializeField]
    private List<FactoryIcon> _npcIcons = new List<FactoryIcon>();
    [SerializeField]
    private List<FactoryIcon> _decorsIcons = new List<FactoryIcon>();
    [SerializeField]
    private List<FactoryIcon> _languageIcons = new List<FactoryIcon>();

    [Space(10f)]
    [Header("Objects")]
    [SerializeField]
    private List<FactoryObject> _levels = new List<FactoryObject>();
    [SerializeField]
    private List<FactoryObject> _building = new List<FactoryObject>();
    [SerializeField]
    private List<FactoryObject> _products = new List<FactoryObject>();
    [SerializeField]
    private List<FactoryObject> _objectsPool = new List<FactoryObject>();
    [SerializeField]
    private List<FactoryObject> _pointLevel = new List<FactoryObject>();
    [SerializeField]
    private List<FactoryObject> _decors = new List<FactoryObject>();

    [Space(10f)]
    [Header("UI")]
    [SerializeField]
    private List<FactoryObject> _dialogs = new List<FactoryObject>();
    [SerializeField]
    private List<FactoryObject> _panels = new List<FactoryObject>();

    [Space(10f)]
    [Header("Sound")]
    [SerializeField]
    private List<FactorySound> _sounds = new List<FactorySound>();

    //=======================================================================================================================
    public void Init() { }

    //=======================================================================================================================
    private GameObject GetObject(string id, EFactoryObjectType objType)
    {
      switch (objType)
      {
        case EFactoryObjectType.Level:
          FactoryObject obj = _levels.Find(t => t.id.Equals(id));
          return obj?.prefab;
        case EFactoryObjectType.Building:
          FactoryObject lo = _building.Find(t => t.id.Equals(id));
          return lo?.prefab;
        case EFactoryObjectType.Product:
          FactoryObject prod = _products.Find(t => t.id.Equals(id));
          return prod?.prefab;
        case EFactoryObjectType.Dialog:
          FactoryObject dialog = _dialogs.Find(t => t.id.Equals(id));
          return dialog?.prefab;
        case EFactoryObjectType.Panel:
          FactoryObject panel = _panels.Find(t => t.id.Equals(id));
          return panel?.prefab;
        case EFactoryObjectType.Pool:
          FactoryObject pool = _objectsPool.Find(t => t.id.Equals(id));
          return pool?.prefab;
        case EFactoryObjectType.PointLevel:
          FactoryObject pointLevel = _pointLevel.Find(t => t.id.Equals(id));
          return pointLevel?.prefab;
        case EFactoryObjectType.Decor:
          FactoryObject custom = _decors.Find(t => t.id.Equals(id));
          return custom?.prefab;
        default:
          return null;
      }
    }

    //=======================================================================================================================
    private Sprite GetSprite(string id, EFactoryObjectType objType)
    {
      switch (objType)
      {
        case EFactoryObjectType.Icon:
          FactoryIcon icon;

          icon = _mainIcons.Find(t => t.id.Equals(id));

          if (icon == null)
          {
            icon = _productIcons.Find(t => t.id.Equals(id));
          }

          if (icon == null)
          {
            icon = _npcIcons.Find(t => t.id.Equals(id));
          }

          if (icon == null)
          {
            icon = _decorsIcons.Find(t => t.id.Equals(id));
          }

          if (icon == null)
          {
            icon = _languageIcons.Find(t => t.id.Equals(id));
          }

          return icon?.sprite;
        default:
          return null;
      }
    }

    //=======================================================================================================================
    public AudioClip GetSound(string id)
    {
      FactorySound sound = _sounds.Find(t => t.Id.Equals(id));
      return sound?.Sound;
    }

    //=======================================================================================================================
    public GameObject InstantiateObject(string id, EFactoryObjectType objType)
    {
      var prefab = GetObject(id, objType);

      if (prefab == null)
        return null;

      var go = Instantiate(prefab);

      if (go == null)
      {
        Debug.Log($"InstantiateObject Instantiate: {id} NULL");
      }

      return go;
    }

    //=======================================================================================================================
    public Sprite InstantiateSprite(string id, EFactoryObjectType objType)
    {
      var prefab = GetSprite(id, objType);

      if (prefab == null)
        return null;

      var go = Instantiate(prefab);

      if (go == null)
      {
        Debug.Log($"InstantiateObject Instantiate: {id} NULL");
      }

      return go;
    }

    //=======================================================================================================================
    public GameObject GetLevel(string id)
    {
      return InstantiateObject(id, EFactoryObjectType.Level);
    }

    //=======================================================================================================================
    public GameObject GetBuilding(string id)
    {
      return InstantiateObject(id, EFactoryObjectType.Building);
    }

    //=======================================================================================================================
    public GameObject GetProduct(string id)
    {
      return InstantiateObject(id, EFactoryObjectType.Product);
    }

    //=======================================================================================================================
    public Sprite GetIcon(string id)
    {
      return InstantiateSprite(id, EFactoryObjectType.Icon);
    }

    //=======================================================================================================================
    public GameObject GetDialog(string id)
    {
      return InstantiateObject(id, EFactoryObjectType.Dialog);
    }

    //=======================================================================================================================
    public GameObject GetPanel(string id)
    {
      return InstantiateObject(id, EFactoryObjectType.Panel);
    }

    //=======================================================================================================================
    public GameObject GetObjectPool(string id)
    {
      return InstantiateObject(id, EFactoryObjectType.Pool);
    }

    //=======================================================================================================================
    public GameObject GetPointLevel(string id)
    {
      return InstantiateObject(id, EFactoryObjectType.PointLevel);
    }
    
    //=======================================================================================================================
    public GameObject GetDecor(string id)
    {
      return InstantiateObject(id, EFactoryObjectType.Decor);
    }

  }
}
