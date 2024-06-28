using UnityEngine;
using UnityEngine.UI;

public class LobbyMainUI : MonoBehaviour
{
  [SerializeField]
  private Button _btnPlay;
  [SerializeField]
  private Button _btnShop;

  // ===================================================================================================
  private void OnEnable()
  {
    _btnPlay.onClick.AddListener(OnBtnPlayClick);
    _btnShop.onClick.AddListener(OnBtnShopClick);
  }

  // ===================================================================================================
  private void OnDisable()
  {
    _btnPlay.onClick.RemoveListener(OnBtnPlayClick);
    _btnShop.onClick.RemoveListener(OnBtnShopClick);
  }

  // ===================================================================================================
  public void Init()
  {
    Global.Instance.SoundService.PlaySound("lobby");
  }

  // ===================================================================================================
  public void OnBtnPlayClick()
  {
        Global.Instance.Game.StartPreBattleState();
  }

  // ===================================================================================================
  public void OnBtnShopClick()
  {
    DialogSystem.Instance.Get<DialogShop>()?.Activate();
  }
}
