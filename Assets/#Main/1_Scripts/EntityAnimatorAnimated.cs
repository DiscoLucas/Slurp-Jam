using UnityEngine;

/// <summary>
/// Entity animator for sprites with animations using Unity's Animator component.
/// Plays different animation states based on direction.
/// </summary>
public class EntityAnimatorAnimated : EntityAnimtor
{
    [System.Serializable]
    public class AnimationNames
    {
        [Tooltip("Animation for forward direction (0-45° and 315-360°)")]
        public string forward = "Forward";
        [Tooltip("Animation for back direction (135-225°)")]
        public string back = "Back";
        [Tooltip("Animation for left direction (225-315°)")]
        public string left = "Left";
        [Tooltip("Animation for right direction (45-135°)")]
        public string right = "Right";
    }

    public bool mutlipuleAnimation = true;

    [SerializeField]
    private AnimationNames animationNames = new AnimationNames();

    private Animator animator;
    private string currentAnimation;

    protected override void Start()
    {
        base.Start();
        
        if (spriteObject != null)
        {
            animator = spriteObject.GetComponent<Animator>();
            
            if (animator == null)
            {
                Debug.LogError($"EntityAnimatorAnimated on {gameObject.name} requires an Animator component on the sprite object!", this);
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if(!mutlipuleAnimation)
            return;
            
        string animation = GetAnimationFromAngle(currentAngle);
        
        if (animation != currentAnimation && animator != null)
        {
            currentAnimation = animation;
            animator.Play(animation);
        }
    }

    private string GetAnimationFromAngle(float angle)
    {
        if (angle >= ANGLE_FORWARD_MIN || angle < ANGLE_FORWARD_MAX)
            return animationNames.forward;
        if (angle >= ANGLE_RIGHT_MIN && angle < ANGLE_RIGHT_MAX)
            return animationNames.right;
        if (angle >= ANGLE_BACK_MIN && angle < ANGLE_BACK_MAX)
            return animationNames.back;
        return animationNames.left;
    }
}
