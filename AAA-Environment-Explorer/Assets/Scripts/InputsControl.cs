using UnityEngine;
using UnityEngine.InputSystem;

public class InputsControl : MonoBehaviour
{
    //Player Input Actions
    InputAction Move;
    InputAction Jump;

    //Reading Inputs Values
    private Vector2 _moveInput;
    private float _jumpAmount;
    private bool _jump;

    private void Awake()
    {
        //Find Input Actions from defaul Input system.
        Move = InputSystem.actions.FindAction("Move");
        Jump = InputSystem.actions.FindAction("Jump");
    }
    private void OnEnable()
    {
        Move.performed += OnMove;
        Move.canceled += OnMove;
        Move.Enable();
        Jump.started += OnJump;
        Jump.canceled += OnJump;
        Jump.Enable();
    }
    void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    void OnJump(InputAction.CallbackContext context)
    {
        _jumpAmount = context.ReadValue<float>();
        if (_jumpAmount>0f)
        {
            _jump = true;
        }
        else if(_jumpAmount==0f)
        {
            _jump = false;
        }
    }
    private void OnDisable()
    {
        Move.performed -= OnMove;
        Move.canceled -= OnMove;
        Move.Disable();
        Jump.started -= OnJump;
        Jump.canceled -= OnJump;
        Jump.Disable();
    }
    //Read Value from Other Scripts.
    public Vector2 MoveInput
    {
        get { return _moveInput; }
    }
    public bool Jumping
    {
        get { return _jump; }
    }
}
