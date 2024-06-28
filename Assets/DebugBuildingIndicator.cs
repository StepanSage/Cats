using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugBuildingIndicator : MonoBehaviour
{
  [SerializeField]
  private GameObject _complete;
  [SerializeField]
  private GameObject _idle;
  [SerializeField]
  private GameObject _progress;

  [SerializeField]
  private Image _icon;
  [SerializeField]
  private TextMeshProUGUI _state;
  [SerializeField]
  private TextMeshProUGUI _produce;
  [SerializeField]
  private Image _progressIcon;

  private ProduceBehaviour _beh;
  private Transform _transform;

  public void Activate(ProduceBehaviour beh)
  {
    _beh = beh;
    _transform = GetComponent<Transform>();

    Global.Instance.Game.OneSecondTimer.Update += OneSecondUpdate;
    Global.Instance.Game.Timer.Update += UpdateTimer;
    OneSecondUpdate();
  }

  // ===================================================================================================
  private void OnDestroy()
  {
    if (Global.Instance != null && Global.Instance.Game != null)
    {
      Global.Instance.Game.OneSecondTimer.Update -= OneSecondUpdate;
      Global.Instance.Game.Timer.Update -= OneSecondUpdate;
    }
  }

  // ===================================================================================================
  void UpdateTimer()
  {
    _transform.position = _beh.View.Transform.position.WorldToScreenPoint();
  }

  // ===================================================================================================
  void OneSecondUpdate()
  {
    _state.text = _beh.State.ToString();

    _progress.SetActive(_beh.IsInProgress);
    _idle.SetActive(_beh.IsIdle);
    _complete.SetActive(_beh.IsComplete  || _beh.IsInProgress);

    if (_beh.IsComplete || _beh.IsInProgress)
    {
      _icon.sprite = Factory.Instance.GetIcon(_beh.Slots[0].FinishConsume.Type);
    }

    if (_beh.IsInProgress)
    {
      _icon.sprite = Factory.Instance.GetIcon(_beh.Slots[0].FinishConsume.Type);
      var elas = _beh.Slots[0].CompleteTimer.Elapsed();
      var rem = Mathf.FloorToInt(_beh.Slots[0].CompleteTimer.Remaining());
      _produce.text = ProjectUtils.ParseTimeGood(rem);
      _progressIcon.fillAmount = rem / _beh.Slots[0].CompleteTimer.Duration;
    }
  }
}
