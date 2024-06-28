using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelExp : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI _count;
  [SerializeField]
  private Transform _transform;
  [SerializeField]
  private Slider _slider;

  private const string _expName = "exp";
  private const string _ratingPointName = "rating_point";

  // ===================================================================================================
  private void OnEnable()
  {
    UIUtils.SetGetter(_expName, _transform);
    UIUtils.SetGetter(_ratingPointName, _transform);
    EventManager.Instance?.AddListener<E_ExpUpdate>(OnExpUpdate);
    Redraw(0, false);
  }

  // ===================================================================================================
  public void OnDisable()
  {
    EventManager.Instance?.RemoveListener<E_ExpUpdate>(OnExpUpdate);
  }

  // ===================================================================================================
  private void OnExpUpdate(E_ExpUpdate evt)
  {
    Redraw(evt.Consume.Amount, true);
  }

  // ===================================================================================================
  private void Redraw(int delta, bool withAnim)
  {
    var exp = PlayerUtils.Data.Experience;
    var expMax = PlayerUtils.ExperienceMax;

    var oldExpValue = (exp - delta) * 1f / expMax ;
    var newExpValue = exp * 1f / expMax ;

    _slider.value = oldExpValue;

    if (!withAnim)
    {
      _slider.value = newExpValue;
      _count.text = $"{(Mathf.FloorToInt(exp))}/{expMax}";
      return;
    }

    _slider.DOValue(newExpValue, 2f)
           .OnUpdate(() =>
           {
             _count.text = $"{(Mathf.FloorToInt(_slider.value * expMax))}/{expMax}";
           });

    _transform.DORewind();
    _transform.localScale = Vector3.one;
    _transform.DOScale(Vector3.one * 1.1f, 0.2f)
      .SetEase(Ease.Linear)
      .SetLoops(1, LoopType.Yoyo)
      .ChangeStartValue(Vector3.one)
      .OnComplete(() => { _transform.localScale = Vector3.one; });
  }
}
