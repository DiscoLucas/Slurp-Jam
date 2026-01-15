using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class StandardTurret : MonoBehaviour
{
    [Header("Turret Stats")]
    [SerializeField] float fireRate = 1f;
    [SerializeField] float range = 10f;

    [Header("Idle Movement")]
    [SerializeField] float rotationSpeed = 60f; // degrees per second
    [SerializeField] float minIdleWait = 1f;
    [SerializeField] float maxIdleWait = 3f;
    [SerializeField] float jitterAmplitude = 5f; // degrees of small non-linear jitter
    [SerializeField] float jitterFrequency = 1f; // frequency for jitter

    Coroutine idleCoroutine;
    
    [Header("Attack")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] string enemyTag = "Enemy";

    Projectile Projectile;

    Transform target;
    float fireCooldown;

    public State currentState;
    public enum State
    {
        Idle,
        Attacking
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = State.Idle;
        if (currentState == State.Idle)
        {
            idleCoroutine = StartCoroutine(IdleBehavior());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Detection and state transitions run in FixedUpdate for consistency with physics
        if (currentState == State.Idle)
        {
            FindAndLockTarget();
        }

        if (currentState == State.Attacking)
        {
            if (target == null)
            {
                // target lost or destroyed
                currentState = State.Idle;
                StartIdleIfNeeded();
                return;
            }

            float dist = Vector3.Distance(transform.position, target.position);
            if (dist > range)
            {
                // out of range, release target
                target = null;
                currentState = State.Idle;
                StartIdleIfNeeded();
                return;
            }

            // Rotate to face target (yaw only)
            Vector3 dir = target.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(dir);
                Quaternion targetRot = Quaternion.Euler(transform.eulerAngles.x, look.eulerAngles.y, transform.eulerAngles.z);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            // Shooting
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                ShootAtTarget();
                fireCooldown = 1f / Mathf.Max(0.0001f, fireRate);
            }
        }
    }

    void FindAndLockTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        Transform closest = null;
        float bestDist = float.MaxValue;
        foreach (var c in hits)
        {
            if (!c.CompareTag(enemyTag)) continue;
            float d = (c.transform.position - transform.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                closest = c.transform;
            }
        }

        if (closest != null)
        {
            // Lock onto this enemy
            target = closest;
            currentState = State.Attacking;
            // stop idle behaviour
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }
        }
    }

    void StartIdleIfNeeded()
    {
        if (idleCoroutine == null && currentState == State.Idle)
        {
            idleCoroutine = StartCoroutine(IdleBehavior());
        }
    }

    // Looks around at random intervals when idle
    IEnumerator IdleBehavior()
    {
        while (currentState == State.Idle)
        {
            Vector3 euler = transform.eulerAngles;
            float startYaw = euler.y;
            float targetYaw = startYaw + Random.Range(-120f, 120f);
            float angleDiff = Mathf.DeltaAngle(startYaw, targetYaw);
            float duration = Mathf.Max(0.01f, Mathf.Abs(angleDiff) / rotationSpeed);
            float elapsed = 0f;
            float seed = Random.value * 10f;

            while (elapsed < duration && currentState == State.Idle)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = Mathf.SmoothStep(0f, 1f, t);
                float yaw = Mathf.LerpAngle(startYaw, targetYaw, eased);
                float jitter = (Mathf.PerlinNoise(seed, Time.time * jitterFrequency) - 0.5f) * 2f * jitterAmplitude * (1f - Mathf.Abs(2f * t - 1f));
                yaw += jitter;
                transform.rotation = Quaternion.Euler(euler.x, yaw, euler.z);
                yield return null;
            }

            // Snap to exact target yaw
            if (currentState != State.Idle) yield break;
            transform.rotation = Quaternion.Euler(euler.x, targetYaw, euler.z);

            // Wait at target with a small sway
            float wait = Random.Range(minIdleWait, maxIdleWait);
            float timer = 0f;
            while (timer < wait && currentState == State.Idle)
            {
                timer += Time.deltaTime;
                float sway = Mathf.Sin(Time.time * jitterFrequency * 2f) * jitterAmplitude * 0.5f;
                transform.rotation = Quaternion.Euler(euler.x, targetYaw + sway, euler.z);
                yield return null;
            }
        }
    }

    void OnDisable()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
    }

    void ShootAtTarget()
    {
        if (bulletPrefab == null || firePoint == null || target == null) return;
        Vector3 dir = (target.position - firePoint.position).normalized;
        GameObject proj = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
    
        // Use the Projectile class properly
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Speed = projectileSpeed;
            projectile.SetDirection(dir, Quaternion.LookRotation(dir), true);
        }
    }

}
