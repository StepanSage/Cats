using DG.Tweening;
using TMPro;
using UnityEngine;

public class PanelCrystal : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI _count;
  [SerializeField]
  private Transform _transform;

  private const string _hardCurrencyName = "crystal";

  // ===================================================================================================
  public void OnEnable()
  {
    UIUtils.SetGetter(_hardCurrencyName, _transform);
    EventManager.Instance?.AddListener<E_CrystalUpdate>(OnCrystalUpdate);
    Redraw(false);
  }

  // ===================================================================================================
  public void OnDisable()
  {
    EventManager.Instance?.RemoveListener<E_CrystalUpdate>(OnCrystalUpdate);
  }

  // ===================================================================================================
  public void OnCrystalUpdate(E_CrystalUpdate evt)
  {
    Redraw(true);
  }

  // ===================================================================================================
  private void Redraw(bool withAnim)
  {
    _count.text = ProjectUtils.GetConsumeValue(_hardCurrencyName).ToString();

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
