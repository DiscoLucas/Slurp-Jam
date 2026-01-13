using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 15f;
    
    [Header("Gravity")]
    [SerializeField] private float gravityScale = -1f;
    
    private CharacterController characterController;
    private Vector2 movementInput;
    private Vector3 velocity = Vector3.zero;
    private InputAction moveAction;
    
    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
        
        if (characterController == null)
        {
            Debug.LogError("CharacterController not found on this GameObject!");
        }
        
        // Setup Input Actions
        //SetupInputActions();
    }
    
    /*void SetupInputActions()
    {
        // Find the input action asset (looking for common naming patterns)
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("Player");
        
        if (inputActionAsset == null)
        {
            Debug.LogWarning("No Input Action has that name);
            return;
        }
        
        // Get the action map and movement action
        var actionMap = inputActionAsset.FindActionMap("Player");
        if (actionMap != null)
        {
            moveAction = actionMap.FindAction("Move");
            if (moveAction != null)
            {
                moveAction.Enable();
            }
        }
    }*/

    void Update()
    {
        HandleMovementInput();
        ApplyMovement();
    }
    
    void HandleMovementInput()
    {
        // Get input from Input Actions
        if (moveAction != null)
        {
            movementInput = moveAction.ReadValue<Vector2>();
        }
        else
        {
            // Fallback to direct input if Input Actions aren't available
            movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
    
    void ApplyMovement()
    {
        // Apply gravity
        velocity.y = Physics.gravity.y * gravityScale;
        
        // Calculate target horizontal velocity
        Vector3 horizontalInput = new Vector3(movementInput.x, 0, movementInput.y);
        Vector3 targetVelocity = horizontalInput.normalized * moveSpeed;
        
        if (horizontalInput.magnitude > 0)
        {
            // Accelerate towards target velocity
            velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, acceleration * Time.deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, acceleration * Time.deltaTime);
        }
        else
        {
            // Decelerate to stop
            velocity.x = Mathf.Lerp(velocity.x, 0, deceleration * Time.deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, 0, deceleration * Time.deltaTime);
        }
        
        // Apply movement using CharacterController
        if (characterController != null)
        {
            characterController.Move(velocity * Time.deltaTime);
        }
    }
    
    void OnDestroy()
    {
        // Clean up input action
        if (moveAction != null)
        {
            moveAction.Disable();
        }
    }
}
