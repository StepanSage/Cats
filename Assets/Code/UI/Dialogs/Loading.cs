using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
  [Header("Loading")]
  [SerializeField]
  private GameObject _contentLoading;
  [SerializeField]
  private Image _mask;

  // ===================================================================================================
  public void Init()
  {
    _contentLoading.SetActive(true);

    _mask.fillAmount = 0;

    _mask.DOFillAmount(1, 3)
        .OnComplete(() =>
        {
          _contentLoading.SetActive(false);
          ButtonTapToStart_Click();
        });
  }

  // ===================================================================================================
  public void ButtonTapToStart_Click()
  {
    SceneManager.LoadScene("Main", LoadSceneMode.Single);
  }
}
