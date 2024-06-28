using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
  public bool IsBussy {get; private set;}
  private Animator _animator;
  private AnimationController _aimationController;

  // ===================================================================================================
  /*
  public void SetWalk(bool isWalking)
  {
    DebugX.LogForBehaviour($"SetWalk {isWalking}");
    
    _animator.SetBool("IsWalk", isWalking);
  }
  */

  // ===================================================================================================
  public void OnCalc()
  {
    DebugX.LogForBehaviour($"OnCalc");

    _animator.SetTrigger("Calc");
  }

  // ===================================================================================================
  public void OnGet()
  {
    DebugX.LogForBehaviour($"OnGet");

    _animator.SetTrigger("Get");
    IsBussy = true;
  }

  // ===================================================================================================
  public void OnWalk()
  {
    DebugX.LogForBehaviour($"OnWalk");
    _animator.SetTrigger("Walk");
  }

  // ===================================================================================================
  public void SetWalk(bool isWalk)
  {
    DebugX.LogForBehaviour($"SetWalk {isWalk}");
    _animator.SetBool("IsWalk", isWalk);
  }

  // ===================================================================================================
  public void OnIdle()
  {
    DebugX.LogForBehaviour($"OnIdle");
    _animator.SetTrigger("Idle");
  }

  // ===================================================================================================
  public void OnUse()
  {
    DebugX.LogForBehaviour($"OnUse");

    _animator.SetTrigger("Use");
    IsBussy = true;
  }

  // ===================================================================================================
  public void OnStopUse()
  {
    DebugX.LogForBehaviour($"OnStopUse");

    _animator.SetTrigger("StopUse");
    IsBussy = false;
  }

  // ===================================================================================================
  public void OnProduce(string producePlayerAnim)
  {
    DebugX.LogForBehaviour($"OnProduce {producePlayerAnim}");

    _animator.SetTrigger(producePlayerAnim);
  }

  // ===================================================================================================
  public void Init()
  {
    _animator = GetComponentInChildren<Animator>();
    _aimationController = GetComponentInChildren<AnimationController>();
    _aimationController.OnGetFinish = OnGetFinish;
    _aimationController.OnUseFinish = OnUseFinish;
  }

  // ===================================================================================================
  public void OnGetFinish()
  {
    DebugX.LogForBehaviour("OnGetFinish");
    
    IsBussy = false;
  }

  // ===================================================================================================
  public void OnUseFinish()
  {
    DebugX.LogForBehaviour("OnUseFinish");
    IsBussy = false;
  }
}