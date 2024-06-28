using Shop;
using System;
using UnityEngine;

[Serializable]
public class ShopTabElement : MonoBehaviour
{
  public Action<EShopTab> OnTabClick;

  [Space(10)]
  [SerializeField]
  private GameObject _Idle;
  [SerializeField]
  private GameObject _Active;
  [SerializeField]
  private EShopTab _type;

  // ===================================================================================================
  public void UpdateState(EShopTab newState)
  {
    _Active.SetActive(_type.Equals(newState));
    _Idle.SetActive(!_type.Equals(newState));
  }

  // ===================================================================================================
  public void OnTab_Click()
  {
    OnTabClick?.Invoke(_type);
  }
}
