using System;
using UnityEngine;

public class View : MonoBehaviour
{
  public Transform Transform { get; private set; }

  [SerializeField]
  private Transform _stayPoint;

  public Action OnObjectMouseDown;
  public Action OnObjectMouseUp;
  public Action OnObjectUpdate;
  public Action OnObjectStart;
  public Action OnObjectEnable;
  public Action OnObjectDisable;
  public Action OnObjectDestroy;
  public Action OnObjectDrawGizmos;

  private bool _isActive = false;

  #region Views
  // ===================================================================================================
  public ProduceView ProduceView
  {
    get
    {
      if (_produceView != null)
      {
        return _produceView;
      }
      else
      {
        var gv = GetComponent<ProduceView>();
        if (gv != null)
        {
          _produceView = gv;
          return _produceView;
        }
        return null;
      }
    }
  }
  private ProduceView _produceView = null;
  
  // ===================================================================================================
  public ProductView ProductView
  {
    get
    {
      if (_productView != null)
      {
        return _productView;
      }
      else
      {
        var gv = GetComponent<ProductView>();
        if (gv != null)
        {
          _productView = gv;
          return _productView;
        }
        return null;
      }
    }
  }
  private ProductView _productView = null;

  // ===================================================================================================
  public CashView CashView
  {
    get
    {
      if (_cashView != null)
      {
        return _cashView;
      }
      else
      {
        var gv = GetComponent<CashView>();
        if (gv != null)
        {
          _cashView = gv;
          return _cashView;
        }
        return null;
      }
    }
  }
  private CashView _cashView = null;

  // ===================================================================================================
  public BinView BinView
  {
    get
    {
      if (_binVieww != null)
      {
        return _binVieww;
      }
      else
      {
        var gv = GetComponent<BinView>();
        if (gv != null)
        {
          _binVieww = gv;
          return _binVieww;
        }
        return null;
      }
    }
  }
  private BinView _binVieww = null;
  #endregion Views

  // ===================================================================================================
  void Awake()
  {
    Transform = transform;
  }

  // ===================================================================================================
  private void Start()
  {
    SetActive(_isActive);
  }

  // ===================================================================================================
  public void SetActive(bool active)
  {
    _isActive = active;
  }

  // ===================================================================================================
  public Transform GetStayPoint()
  {
    return _stayPoint;
  }

  // ===================================================================================================
  void OnEnable()
  {
    OnObjectEnable?.Invoke();
  }

  // ===================================================================================================
  void OnDisable()
  {
    OnObjectDisable?.Invoke();
  }

  // ===================================================================================================
  void OnDestroy()
  {
    OnObjectDestroy?.Invoke();
  }

  // ===================================================================================================
  void OnDrawGizmos()
  {
    OnObjectDrawGizmos?.Invoke();
  }

  // ===================================================================================================
  public void OnMouseDown()
  {
    var overGO = UIUtils.IsPointerOverGameObject();

    if (overGO)
      return;

    OnObjectMouseDown?.Invoke();
  }

  // ===================================================================================================
  public void OnMouseUp()
  {
    var overGO = UIUtils.IsPointerOverGameObject();

    if (overGO)
      return;

    OnObjectMouseUp?.Invoke();
  }

  // ===================================================================================================
  private void Update()
  {
    OnObjectUpdate?.Invoke();
  }
}
