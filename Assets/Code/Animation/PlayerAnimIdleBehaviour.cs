using UnityEngine;

public class PlayerAnimIdleBehaviour : StateMachineBehaviour
{
  [SerializeField]
  private float _timeUntilIdle;

  [SerializeField]
  private int _numberOfIdleAnimations;

  private bool _isIdle;
  private float _idleTime;
  private int _idleAnimation;

  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    ResetIdle();
  }

  // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (!_isIdle)
    {
      _idleTime += Time.deltaTime;
      if ((_idleTime > _timeUntilIdle) && ((stateInfo.normalizedTime % 1) < 0.02f))
      {
        _isIdle = true;
        _idleAnimation = Random.Range(1, _numberOfIdleAnimations + 1);
        _idleAnimation = _idleAnimation * 2 + 1;
        animator.SetFloat("IdleAnim", _idleAnimation - 1);
      }
    }
    else if (stateInfo.normalizedTime % 1 > 0.98)
    {
      ResetIdle();
    }

    animator.SetFloat("IdleAnim", _idleAnimation);
  }

  private void ResetIdle()
  {
    if (_isIdle)
    {
      _idleAnimation--;
    }
      _isIdle = false;
    _idleTime = 0;
  }

  // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  //{
  //    
  //}

  // OnStateMove is called right after Animator.OnAnimatorMove()
  //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  //{
  //    // Implement code that processes and affects root motion
  //}

  // OnStateIK is called right after Animator.OnAnimatorIK()
  //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  //{
  //    // Implement code that sets up animation IK (inverse kinematics)
  //}
}
