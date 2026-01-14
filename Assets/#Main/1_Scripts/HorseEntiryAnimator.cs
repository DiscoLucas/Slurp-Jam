using UnityEngine;
using UnityEngine.AI;

public class HorseEntiryAnimator : MonoBehaviour
{
 [Header("References")]
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    
    [SerializeField]
    private Animator animator;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    EntityAnimatorStatic entityAnimatorStatic;

    [Header("Settings")]
    [SerializeField]
    private float baseSpeed = 3.5f;

    [SerializeField]
    private float minAnimationSpeed = 0f;

    [SerializeField]
    private float maxAnimationSpeed = 3f;

    [SerializeField]
    private float smoothing = 5f;

    private float currentAnimationSpeed = 1f;

    private void Start()
    {
        baseSpeed = navMeshAgent.speed;
    }

    private void Update()
    {
        if (navMeshAgent == null || animator == null)
            return;

        float agentSpeed = navMeshAgent.velocity.magnitude;
        float targetAnimationSpeed = agentSpeed / baseSpeed;
        targetAnimationSpeed = Mathf.Clamp(targetAnimationSpeed, minAnimationSpeed, maxAnimationSpeed);

        currentAnimationSpeed = Mathf.Lerp(currentAnimationSpeed, targetAnimationSpeed, Time.deltaTime * smoothing);

        animator.speed = currentAnimationSpeed;

        if(entityAnimatorStatic.GetCurrentDirection() == 1)
        {
           spriteRenderer.flipX = true; 
        }else if(entityAnimatorStatic.GetCurrentDirection() == 3)
        {  
            spriteRenderer.flipX = false;
        }
            
    }
}
