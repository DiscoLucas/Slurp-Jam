using UnityEngine;
using UnityEngine.InputSystem;

public class MeatGrinder : MonoBehaviour
{
    [SerializeField]
    protected GameObject dropItemPrefab;
    [SerializeField]
    protected Transform grinedObject;
    [SerializeField]
    protected Transform dropItemSpawnPoint, grindContainer;
    [SerializeField]
    protected float itemEjectForce = 2f;
    [SerializeField]
    protected ParticleSystem bloodEffect;
    [SerializeField]
    protected ParticleSystem meatchunckEffect;

    [SerializeField]
    protected Animator animator;

    PlayerActions playerActions;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        
    }



    [ContextMenu("Start Grind")]
    public void startGrind()
    {
        animator.SetTrigger("Grind");
    }

    public void startEffect()
    {
        if (bloodEffect != null)
        {
            bloodEffect.Play();
        }
        if (meatchunckEffect != null)
        {
            meatchunckEffect.Play();
        }
    }

    public void dropItem()
    {
        bloodEffect.Stop();
        meatchunckEffect.Stop();
        if(grinedObject != null)
        {
            Destroy(grinedObject.gameObject);
        }
        if (dropItemPrefab != null)
        {
            GameObject obj = Instantiate(dropItemPrefab, dropItemSpawnPoint.position, dropItemSpawnPoint.rotation);
            obj.GetComponent<Rigidbody>().AddForce(dropItemSpawnPoint.forward * itemEjectForce, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("MeatGrinder trigger entered by " + other.name);
        if(other.CompareTag("Player")){
            if(playerActions == null)
                playerActions = other.GetComponent<PlayerActions>();

            if(playerActions != null){
                playerActions.pirrorityInteraction = true;
                Debug.Log("Player in range to interact with MeatGrinder");
                playerActions.Interact.action.started += startGrindWithBody;
                playerActions.possiableInteractEvent.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("MeatGrinder trigger exited by " + other.name);
        if(other.CompareTag("Player")){
            if(playerActions != null){
                playerActions.pirrorityInteraction = false;
                Debug.Log("Player out of range to interact with MeatGrinder");
                playerActions.Interact.action.started -= startGrindWithBody;
                playerActions.unPossiableInteractEvent.Invoke();
            }
        }
    }

    public void startGrindWithBody(InputAction.CallbackContext context)
    {
        if(playerActions.isCarryingObject()){
            GameObject carriedObject = playerActions.removeCarriedObject();
            carriedObject.transform.SetParent(grindContainer);
            carriedObject.transform.localPosition = Vector3.zero;
            carriedObject.transform.localRotation = Quaternion.identity;
            grinedObject = carriedObject.transform;
            startGrind();
        }
    }
}
