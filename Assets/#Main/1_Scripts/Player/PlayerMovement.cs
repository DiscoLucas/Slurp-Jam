using System.Collections;
using System.Runtime.Serialization;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController _characterController;
    public float moveModifier;
    public float dashPower;
    public float dashDuration;
    public float gravityModifier;
    private Vector2 _moveDirection;

    public InputActionReference Move;
    public InputActionReference Dash;

    void Update()
    {
        _moveDirection = Move.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        float gravity = gravityModifier * Time.deltaTime;
        _characterController.Move(new Vector3(_moveDirection.x * moveModifier, gravity, _moveDirection.y * moveModifier));
    }

    void OnEnable()
    {
        Dash.action.started += OnDash;
    }

    void OnDisable()
    {
        Dash.action.started -= OnDash;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        //check if on cooldown
        Debug.Log("Dashing");
        StartCoroutine(DashEnum());
    }

    IEnumerator DashEnum()
    {
        float myTime = dashDuration; 
        float elapsedTime = 0f;

        Vector3 dashDirection =
            new Vector3(_moveDirection.x, 0f, _moveDirection.y).normalized * dashPower;

        while (elapsedTime < myTime)
        {
            _characterController.Move(dashDirection * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
