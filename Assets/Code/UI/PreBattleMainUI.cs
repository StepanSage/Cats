using UnityEngine;
using UnityEngine.UI;

public class PreBattleMainUI : MonoBehaviour
{
    [SerializeField]
    private Button _btnBack;

    private void OnEnable()
    {
        _btnBack.onClick.AddListener(OnBtnBackClick);
    }

    private void OnDisable()
    {
        _btnBack.onClick.RemoveListener(OnBtnBackClick);
    }

    public void Init()
    {
        Global.Instance.SoundService.PlaySound("PreBattle");
    }
    public void OnBtnBackClick()
    {
        Global.Instance.Game.StartLobbyState();
    }
}
