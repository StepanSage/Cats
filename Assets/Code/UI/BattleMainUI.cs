using Assets.Code.UI;
using UnityEngine;
using UnityEngine.UI;

public class BattleMainUI : MonoBehaviour
{
  public OrderUI OrderUI;
  public PlayerSlotsUI PlayerSlotsUI;
  public PauseView PauseView;
  public Button _btnPause;

  private bool _isBtnPauseOn = false;

   private void OnEnable()
   {
      _btnPause.onClick.AddListener(OnBtnPauseClick);
   }

   private void OnDisable()
   {
        _btnPause.onClick.RemoveListener(OnBtnPauseClick);
   }

    // ===================================================================================================
    public void Init()
  {
    Global.Instance.SoundService.PlaySound("battle");
    OrderUI.Init();
    PlayerSlotsUI.Init();
    PauseView.init();
    PauseView.SetStatePause(_isBtnPauseOn);
  }

    private void OnBtnPauseClick()
    {
        _isBtnPauseOn = !_isBtnPauseOn;
        if(_isBtnPauseOn)
        {
            Global.Instance.Game.OneSecondTimer.Stop();
        }
        else
        {
            Global.Instance.Game.OneSecondTimer.Start();
        }
        PauseView.SetStatePause(_isBtnPauseOn);
    }
}
