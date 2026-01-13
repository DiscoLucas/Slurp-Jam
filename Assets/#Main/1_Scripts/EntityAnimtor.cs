using UnityEngine;

/// <summary>
/// Base class for entity sprite/animation rotation based on camera angle.
/// Handles directional detection and sprite object rotation.
/// </summary>
public abstract class EntityAnimtor : MonoBehaviour
{
    [SerializeField]
    protected GameObject spriteObject;

    protected const float ANGLE_FULL_CIRCLE = 360f;
    protected const float ANGLE_FORWARD_MIN = 315f;
    protected const float ANGLE_FORWARD_MAX = 45f;
    protected const float ANGLE_RIGHT_MIN = 45f;
    protected const float ANGLE_RIGHT_MAX = 135f;
    protected const float ANGLE_BACK_MIN = 135f;
    protected const float ANGLE_BACK_MAX = 225f;
    protected const float ANGLE_LEFT_MIN = 225f;
    protected const float ANGLE_LEFT_MAX = 315f;

    protected float currentAngle;

    protected virtual void Start()
    {
        if (spriteObject == null)
            spriteObject = transform.GetChild(0).gameObject;
    }
    
    protected virtual void FixedUpdate()
    {

        Vector3 cameraFoward = Camera.main.transform.forward;
        Vector3 objectFoward = transform.forward;

        float angle = Vector3.SignedAngle(cameraFoward, objectFoward, Vector3.up);

        if (angle < 0) 
            angle += ANGLE_FULL_CIRCLE;

        currentAngle = angle;


        if (spriteObject != null)
        {
            spriteObject.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
        }
    }
}
