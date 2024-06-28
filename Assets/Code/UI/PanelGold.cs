using DG.Tweening;
using TMPro;
using UnityEngine;

public class PanelGold : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI _count;
  [SerializeField]
  private Transform _transform;

  private const string _softCurrencyName = "gold";

  // ===================================================================================================
  public void OnEnable()
  {
    UIUtils.SetGetter(_softCurrencyName, _transform);
    EventManager.Instance?.AddListener<E_GoldUpdate>(OnGoldUpdate);
    Redraw(false);
  }

  // ===================================================================================================
  public void OnDisable()
  {
    EventManager.Instance?.RemoveListener<E_GoldUpdate>(OnGoldUpdate);
  }

  // ===================================================================================================
  public void OnGoldUpdate(E_GoldUpdate evt)
  {
    Redraw(true);
  }

  // ===================================================================================================
  private void Redraw(bool withAnim)
  {
    _count.text = ProjectUtils.GetConsumeValue(_softCurrencyName).ToString();

    if (!withAnim)
      return;

    _transform.DORewind();
    _transform.localScale = Vector3.one;
    _transform.DOScale(Vector3.one * 1.1f, 0.2f)
      .SetEase(Ease.Linear)
      .SetLoops(1, LoopType.Yoyo)
      .ChangeStartValue(Vector3.one)
      .OnComplete(() => { _transform.localScale = Vector3.one; });
  }
}
