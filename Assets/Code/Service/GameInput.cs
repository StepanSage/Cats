using System;
using UnityEngine;

namespace Code.Service
{
    public class GameInput : MonoBehaviour
    {
        public Action InteractionAction;
        public Action<float> MouseDiractionCallBack; 
      
        private Vector2 _inputMove;
        private SettingInputAction _inputActions;
        private bool _isInput = true;

        public void StopInput()
        {
            _isInput = false;
        }

        public void StartInput()
        {
            _isInput = true;
        }

        private void Start()
        {
            _inputActions = new SettingInputAction();
            _inputActions.Enable();

            _inputActions.Player.Interaction.performed += Interaction_performed;
        }

        private void Update()
        {
            MovingMouse();
        }

        private void Interaction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            InteractionAction?.Invoke();
        }

        public Vector2 GetMovingVectorNormalazed()
        {            
            _inputMove = _inputActions.Player.Move.ReadValue<Vector2>();
            _inputMove = _inputMove.normalized;

            return _inputMove;
        }

        private void MovingMouse()
        {
            if(Input.GetMouseButton(0) && _isInput)
            {
                float diraction = Input.GetAxisRaw("Mouse X");
                MouseDiractionCallBack?.Invoke(-diraction);
            }     
        }



    }
}