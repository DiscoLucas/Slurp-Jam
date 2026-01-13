using System.Runtime.Serialization;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController _characterController;
    public float moveModifier;
    public float gravityModifier;
    private Vector2 _moveDirection;

    public InputActionReference Move;

    void Update()
    {
        _moveDirection = Move.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        float gravity = gravityModifier * Time.deltaTime;
        _characterController.Move(new Vector3(_moveDirection.x * moveModifier, gravity, _moveDirection.y * moveModifier));
    }
}
