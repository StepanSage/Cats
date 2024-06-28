using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelPlayer : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI _level;
  [SerializeField]
  private TextMeshProUGUI _levelRating;

  [SerializeField]
  private Button _btnPlayerLevelProgress;
  [SerializeField]
  private Button _btnPlayerRatingLevelProgress;

  // ===================================================================================================
  private void OnEnable()
  {
    EventManager.Instance?.AddListener<E_NewLevelGet>(OnNewLevelGet);
    EventManager.Instance?.AddListener<E_RatingNewLevelGet>(OnRatingNewLevelGet);

    _btnPlayerLevelProgress.onClick.AddListener(OnBtnPlayerLevelProgress_Click);
    _btnPlayerRatingLevelProgress.onClick.AddListener(OnBtnPlayerRatingLevelProgress_Click);

    Redraw();
  }

  // ===================================================================================================
  private void OnDisable()
  {
    EventManager.Instance?.RemoveListener<E_NewLevelGet>(OnNewLevelGet);
    EventManager.Instance?.RemoveListener<E_RatingNewLevelGet>(OnRatingNewLevelGet);

    _btnPlayerLevelProgress.onClick.RemoveListener(OnBtnPlayerLevelProgress_Click);
    _btnPlayerRatingLevelProgress.onClick.RemoveListener(OnBtnPlayerRatingLevelProgress_Click);
  }

  // ===================================================================================================
  private void OnNewLevelGet(E_NewLevelGet evt)
  {
    Redraw();
  }

  // ===================================================================================================
  private void OnRatingNewLevelGet(E_RatingNewLevelGet evt)
  {
    Redraw();
  }

  // ===================================================================================================
  private void Redraw()
  {
    _level.text = Global.Instance.PlayerData.Level.ToString();
    _levelRating.text = Global.Instance.PlayerData.RatingLevel.ToString();
  }

  // ===================================================================================================
  private void OnBtnPlayerLevelProgress_Click()
  {
    DialogSystem.Instance.Get<DialogLevelProgress>()?.Activate();
  }

  // ===================================================================================================
  private void OnBtnPlayerRatingLevelProgress_Click()
  {
    DialogSystem.Instance.Get<DialogRatingLevelProgress>()?.Activate();
  }
}
