using Code.Service;
using UnityEngine;
using Zenject.SpaceFighter;

namespace Code.Logic.Player
{
  public class Player : MonoBehaviour
  {
    public bool IsWalking() => _isWalking;

    [SerializeField]
    private float _moveSpeed, _rotationSpeed, _distanceRay = 7f;
    [SerializeField]
    private GameInput _gameInput;

    private bool _isWalking;
    private bool canMove;
    private Vector3 _moveDirection;
    private Vector3 _lastInteractionDiraction;
    private ClearCounter _clearCounter;
    private SelectedObject _selectedObject;
    private bool isSelected = false;

    // ===================================================================================================
    private void Awake()
    {
      _gameInput = Global.Instance.Game.GameInput;
      //Global.Instance.Game.SetPlayer(this);
    }

    // ===================================================================================================
    private void OnEnable()
    {
      _gameInput.InteractionAction += CheckClreatCounter;
    }

    // ===================================================================================================
    private void OnDisable()
    {
      _gameInput.InteractionAction -= CheckClreatCounter;
    }

    // ===================================================================================================
    private void Update()
    {
      CheckMoveDirectionForInteraction();
      HandleInteraction();
      Move();
      CheckStateSelected();
      Rotation();
    }

    // ===================================================================================================
    public void Move()
    {
      Vector2 inputVectorNormaleze = _gameInput.GetMovingVectorNormalazed();

      _moveDirection = new Vector3(inputVectorNormaleze.x, 0, inputVectorNormaleze.y);

      CollisionCheck();

      if (canMove)
        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;

      _isWalking = _moveDirection != Vector3.zero && canMove;
    }

    // ===================================================================================================
    public void Rotation()
    {
      transform.forward = Vector3.Slerp(transform.forward, -_moveDirection, Time.deltaTime * _rotationSpeed);
    }

    // ===================================================================================================
    public bool CollisionCheck()
    {
      float _playerRadius = 0.5f;
      float _playerHight = 0.5f;

      canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHight, _playerRadius, _moveDirection, _distanceRay);
      return canMove;
    }

    // ===================================================================================================
    private void CheckMoveDirectionForInteraction()
    {
      if (_moveDirection != Vector3.zero)
        _lastInteractionDiraction = _moveDirection;
    }

    // ===================================================================================================
    private void CheckClreatCounter()
    {
      if (_clearCounter != null)
      {
        _clearCounter.Interaction();
      }
    }

    // ===================================================================================================
    private void CheckStateSelected()
    {
      if (_selectedObject != null)
        _selectedObject.currentStateSelected(isSelected);
    }

    // ===================================================================================================
    public void HandleInteraction()
    {
      RaycastHit _raycasHit;
      float _distanceRayCast = 1.5f;

      if (Physics.Raycast(transform.position, _lastInteractionDiraction, out _raycasHit, _distanceRayCast))
      {
        if (_raycasHit.transform.TryGetComponent(out SelectedObject selectedObject))
        {
          isSelected = true;
          _selectedObject = selectedObject;
        }
        else
        {
          isSelected = false;
        }

        if (_raycasHit.transform.TryGetComponent(out ClearCounter clearCounter))
        {
          _clearCounter = clearCounter;
        }
        else
        {
          _clearCounter = null;
        }
      }
      else
      {
        _clearCounter = null;
        isSelected = false;
      }
    }
  }
}
