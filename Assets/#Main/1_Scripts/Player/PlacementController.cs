using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementController : MonoBehaviour
{
    [Header("Placement Settings")]
    [SerializeField] float maxPlaceDistance = 4f;
    [SerializeField] float gridSize = 0.25f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask blockingMask;
    [SerializeField] InputActionReference mousePositionAction;

    public bool IsPlacing => isPlacing;

    GameObject ghostInstance;
    GameObject livePrefab;
    Material validMaterial;
    Material invalidMaterial;
    bool isPlacing;
    bool currentValid;
    Vector3 currentPosition;
    Quaternion currentRotation;
    Bounds ghostLocalBounds;
    readonly List<Renderer> ghostRenderers = new List<Renderer>();
    readonly HashSet<Collider> playerColliders = new HashSet<Collider>();

    void Awake()
    {
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            if (col != null)
                playerColliders.Add(col);
        }
    }

    void Update()
    {
        if (!isPlacing)
            return;

        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelPlacement();
            return;
        }

        if (!TryGetMouseWorld(out var hitPos))
            return;

        currentRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        currentPosition = SnapToGrid(hitPos);
        currentValid = ValidatePosition(currentPosition, currentRotation);

        UpdateGhostTransform();
        UpdateGhostMaterial();
    }

    public void BeginPlacement(GameObject prefab, Material validMat, Material invalidMat)
    {
        CancelPlacement();
        if (prefab == null)
            return;

        livePrefab = prefab;
        validMaterial = validMat;
        invalidMaterial = invalidMat;

        ghostInstance = Instantiate(prefab);
        ghostInstance.name = prefab.name + "_Ghost";
        DisableBehaviours(ghostInstance);
        CacheRenderers(ghostInstance);
        CacheBounds();
        UpdateGhostMaterial();
        isPlacing = true;
    }

    public void CancelPlacement()
    {
        isPlacing = false;
        currentValid = false;
        livePrefab = null;
        if (ghostInstance != null)
            Destroy(ghostInstance);
        ghostInstance = null;
        ghostRenderers.Clear();
    }

    public void TryConfirmPlacement()
    {
        if (!isPlacing || !currentValid || livePrefab == null)
            return;

        Instantiate(livePrefab, currentPosition, currentRotation);
        CancelPlacement();
    }

    bool TryGetMouseWorld(out Vector3 point)
    {
        point = default;
        Camera cam = Camera.main;
        if (cam == null)
            return false;

        Vector2 screenPos = Vector2.zero;
        if (mousePositionAction != null && mousePositionAction.action != null)
            screenPos = mousePositionAction.action.ReadValue<Vector2>();
        else if (Mouse.current != null)
            screenPos = Mouse.current.position.ReadValue();

        Ray ray = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit, 500f, groundMask, QueryTriggerInteraction.Ignore))
        {
            point = hit.point;
            return true;
        }

        return false;
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3
        (
            Mathf.Round(pos.x / gridSize) * gridSize,
            pos.y,
            Mathf.Round(pos.z / gridSize) * gridSize
        );
    }

    bool ValidatePosition(Vector3 pos, Quaternion rot)
    {
        float maxDistSqr = maxPlaceDistance * maxPlaceDistance;
        if ((pos - transform.position).sqrMagnitude > maxDistSqr)
            return false;

        Vector3 worldCenter = pos + rot * ghostLocalBounds.center;
        Vector3 halfExtents = ghostLocalBounds.extents;
        if (halfExtents == Vector3.zero)
            halfExtents = Vector3.one * 0.25f;

        var hits = Physics.OverlapBox(worldCenter, halfExtents, rot, blockingMask, QueryTriggerInteraction.Ignore);
        foreach (var hit in hits)
        {
            if (hit == null)
                continue;
            if (playerColliders.Contains(hit))
                continue;
            return false;
        }

        return true;
    }

    void UpdateGhostTransform()
    {
        if (ghostInstance == null)
            return;

        ghostInstance.transform.SetPositionAndRotation(currentPosition, currentRotation);
    }

    void UpdateGhostMaterial()
    {
        Material target = currentValid ? validMaterial : invalidMaterial;
        if (target == null)
            return;

        for (int i = 0; i < ghostRenderers.Count; i++)
        {
            var r = ghostRenderers[i];
            if (r == null)
                continue;

            var mats = r.sharedMaterials;
            for (int m = 0; m < mats.Length; m++)
                mats[m] = target;
            r.sharedMaterials = mats;
        }
    }

    void CacheRenderers(GameObject root)
    {
        ghostRenderers.Clear();
        ghostRenderers.AddRange(root.GetComponentsInChildren<Renderer>());
    }

    void CacheBounds()
    {
        if (ghostRenderers.Count == 0)
        {
            ghostLocalBounds = new Bounds(Vector3.zero, Vector3.one * 0.5f);
            return;
        }

        Bounds worldBounds = ghostRenderers[0].bounds;
        for (int i = 1; i < ghostRenderers.Count; i++)
            worldBounds.Encapsulate(ghostRenderers[i].bounds);

        ghostLocalBounds = new Bounds
        {
            center = ghostInstance.transform.InverseTransformPoint(worldBounds.center),
            extents = worldBounds.extents
        };
    }

    void DisableBehaviours(GameObject root)
    {
        foreach (var mb in root.GetComponentsInChildren<MonoBehaviour>())
        {
            if (mb != null)
                mb.enabled = false;
        }

        foreach (var col in root.GetComponentsInChildren<Collider>())
        {
            if (col != null)
                col.enabled = false;
        }

        foreach (var rb in root.GetComponentsInChildren<Rigidbody>())
        {
            if (rb != null)
                rb.isKinematic = true;
        }
    }

    void OnDisable()
    {
        CancelPlacement();
    }
}
