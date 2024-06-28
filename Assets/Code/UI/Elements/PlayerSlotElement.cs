using Objects;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotElement : MonoBehaviour
{
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _bgActive;
  [SerializeField]
  private Image _bgEmpty;

  private PlayerSlot _playerSlot;

  // ===================================================================================================
  public void Awake()
  {
    _bgActive.gameObject.SetActive(false);
    _bgEmpty.gameObject.SetActive(true);
    _icon.gameObject.SetActive(false);
  }

  // ===================================================================================================
  public void Init(PlayerSlot playerSlot)
  {
    if (playerSlot == null)
    {
      gameObject?.SetActive(false);
      return;
    }

    _playerSlot = playerSlot;
    Redraw();
  }

  // ===================================================================================================
  public void Redraw()
  {
    if (_playerSlot.Consume == null)
    {
      _bgActive.gameObject.SetActive(false);
      _bgEmpty.gameObject.SetActive(true);
      _icon.gameObject.SetActive(false);
    }
    else
    {
      _icon.sprite = Factory.Instance.GetIcon(_playerSlot.Consume.Type);
      _bgActive.gameObject.SetActive(true);
      _bgEmpty.gameObject.SetActive(false);
      _icon.gameObject.SetActive(true);
    }

    gameObject?.SetActive(true);
  }
}
