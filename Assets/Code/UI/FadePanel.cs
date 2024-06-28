using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
  [SerializeField]
  private Image _bg;

  void OnEnable()
    {
    _bg.color = new Color(0f, 0f, 0f, 0.5f);

    _bg.gameObject.SetActive(true);
    _bg.DOFade(0, 1)
       .OnComplete(() => { _bg.gameObject.SetActive(false); });
    }
}
