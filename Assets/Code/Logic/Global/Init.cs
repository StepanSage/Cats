using UnityEngine;

public class Init : MonoBehaviour
{
  public InitUI InitUI;

  // ===================================================================================================
  void Start()
  {
    //Application.OpenURL($@"{Application.dataPath}");
    
    DebugX.Init();
    StartInit();
  }

  // ===================================================================================================
  /// <summary>
  /// Init all Mechanics
  /// </summary>
  private void StartInit()
  {
    LoadStatic();

    DebugX.LogWarning($"LoadStatic");

    LoadPlayer();
    InitUI.Init();
  }

  // ===================================================================================================
  /// <summary>
  /// Load StaticData
  /// </summary>
  private void LoadStatic()
  {
    Global.Instance.StaticData = GddUtils.GetStatic();
  }

  // ===================================================================================================
  /// <summary>
  /// Load Player
  /// </summary>
  private void LoadPlayer()
  {
    DataStoreManager.Instance.Init(
      () =>
      {
        Global.Instance.PlayerData = DataStoreManager.Instance.Data.PlayerData;
      },
      () =>
      {
        Global.Instance.PlayerData = DataStoreManager.Instance.Data.PlayerData;
        PlayerUtils.Init();
      });
  }
}
