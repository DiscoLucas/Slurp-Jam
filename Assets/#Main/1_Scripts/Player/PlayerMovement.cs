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
    public float dashCooldown = 1;
    private bool canDash = true;
    public InputActionReference MousePosition;

    void Update()
    {
        _moveDirection = Move.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        float gravity = gravityModifier * Time.deltaTime;
        _characterController.Move(new Vector3(_moveDirection.x * moveModifier, gravity, _moveDirection.y * moveModifier));

        Vector3 mousePos = MousePosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.transform.position.y - transform.position.y;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.y = transform.position.y;
        transform.LookAt(worldPos);

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
        if(canDash)
            StartCoroutine(DashEnum());
    }

    IEnumerator DashEnum() //will currently dash for x seconds THEN wait for x seconds cooldown.
    {
        canDash = false;
        float myTime = dashDuration; 
        float elapsedTime = 0f;
        float dashEnumCooldown = dashCooldown;

        Vector3 dashDirection =
            new Vector3(_moveDirection.x, 0f, _moveDirection.y).normalized * dashPower;

        while (elapsedTime < myTime)
        {
            _characterController.Move(dashDirection * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (dashEnumCooldown > 0)
        {
            dashEnumCooldown -= Time.deltaTime;
            yield return null;
        }
        canDash = true;
        yield return null;
    }
}
