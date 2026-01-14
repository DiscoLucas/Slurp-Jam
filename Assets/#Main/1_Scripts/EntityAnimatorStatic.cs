using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Entity animator for static sprites (single-frame) using SpriteRenderer.
/// Swaps sprites based on direction.
/// </summary>
public class EntityAnimatorStatic : EntityAnimtor
{
    [Header("Static Sprites")]
    [SerializeField]
    [Tooltip("Sprite to display when facing forward (0-45° and 315-360°)")]
    private Sprite forwardSprite;
    
    [SerializeField]
    [Tooltip("Sprite to display when facing back (135-225°)")]
    private Sprite backSprite;
    
    [SerializeField]
    [Tooltip("Sprite to display when facing left (225-315°)")]
    private Sprite leftSprite;
    
    [SerializeField]
    [Tooltip("Sprite to display when facing right (45-135°)")]
    private Sprite rightSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite currentSprite;
    int currentDirection = 0;

    protected override void Start()
    {
        base.Start();
        
        if (spriteObject != null)
        {
            spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            
            if (spriteRenderer == null)
            {
                Debug.LogError($"EntityAnimatorStatic on {gameObject.name} requires a SpriteRenderer component on the sprite object!", this);
            }
        }
    }
    public int GetCurrentDirection(){
        return currentDirection;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        Sprite sprite = GetSpriteFromAngle(currentAngle);
        
        if (sprite != currentSprite && spriteRenderer != null && sprite != null)
        {
            currentSprite = sprite;
            spriteRenderer.sprite = sprite;
        }
    }

    private Sprite GetSpriteFromAngle(float angle)
    {
        if (angle >= ANGLE_FORWARD_MIN || angle < ANGLE_FORWARD_MAX)
        {
            currentDirection = 0;
            return forwardSprite;
        }            
        if (angle >= ANGLE_RIGHT_MIN && angle < ANGLE_RIGHT_MAX)
        {
            currentDirection = 1;
            return rightSprite;
        }
        if (angle >= ANGLE_BACK_MIN && angle < ANGLE_BACK_MAX)
        {
            currentDirection = 2;
            return backSprite;
        }
        currentDirection = 3;
        return leftSprite;
    }
}
