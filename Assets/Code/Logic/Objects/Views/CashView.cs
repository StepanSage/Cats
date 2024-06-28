using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CashView : MonoBehaviour 
{
  [SerializeField]
  private List<CashOrderView> _cashOrderView;

  // ===================================================================================================
  public CashOrderView GetCashOrderViewByIndex(int index)
  {
    return _cashOrderView[index];
  }
}
