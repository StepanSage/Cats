#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class Helpers : MonoBehaviour
{

  #region GDD
  // ===================================================================================================
  [MenuItem("Tools/⚙️ Helpers/GDD/Load Static", priority = 998)]
  public static void LoadStatic()
  {
    GddUtils.ImportStatic();
  }
  #endregion GDD

}
#endif