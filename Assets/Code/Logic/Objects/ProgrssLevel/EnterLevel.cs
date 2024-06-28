using Code.ProgressLevel;
using UnityEngine;

public class EnterLevel : MonoBehaviour
{
    private SelectUtilits _selectUtilits;

    [HideInInspector] public bool IsActivate = true;

  // ===================================================================================================
  private void OnMouseDown()
  {
    if(IsActivate)
    {
       DialogSystem.Instance?.Get<DialogStartLevel>()?.Initialized(gameObject.GetComponent<Level>().AmountStar);
       DialogSystem.Instance?.Get<DialogStartLevel>()?.Activate();
       Global.Instance.Game.GameInput.StopInput();
       Global.Instance.SoundService.PlayOne("popup");     
    }
    
  }

    private void OnMouseEnter()
    {
        _selectUtilits = GetComponent<SelectUtilits>();

        if (_selectUtilits != null  && IsActivate)
        {  
            Global.Instance.SoundService.PlayOne("SoundFlag");
            _selectUtilits.Select();
        }
    }

    private void OnMouseExit()
    {
        if (_selectUtilits != null)
        {
            _selectUtilits.UNSelecet();
        }
    }
}
