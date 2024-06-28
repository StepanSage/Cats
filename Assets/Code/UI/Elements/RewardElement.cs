using Objects;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardElement : MonoBehaviour
{
  [SerializeField]
  private Image _icon = null;
  [SerializeField]
  private TextMeshProUGUI _text;

  private Consume _consume;

  // ===================================================================================================
  public void Init(Consume consume)
  {
    if (consume == null)
      return;

    gameObject.SetActive(true);
    _consume = consume;
     Redraw();
  }

  // ===================================================================================================
  public void Redraw()
  {
    _icon.sprite = Factory.Instance.GetIcon(_consume.Type);
    _text.text = _consume.Amount.ToString();
    transform.UpdateLayout();
  }
}
