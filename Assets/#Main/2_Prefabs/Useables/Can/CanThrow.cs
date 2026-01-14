using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeaponTemplate : MonoBehaviour
{
    public float lifeTime = 0.5f;
    public int damage;
    public float travelSpeed;
    public float hitStrenghThreshold = 1f;
    Vector3 direction;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        direction = (transform.position - GameObject.FindWithTag("Player").transform.position).normalized;

        rb.AddForce(direction*travelSpeed, ForceMode.Impulse);
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(GetComponentInParent<GameObject>());
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && rb.linearVelocity.magnitude > hitStrenghThreshold)
        {
            EnemytClass enemy = collision.gameObject.GetComponent<EnemytClass>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(damage);
            }

            Destroy(GetComponentInParent<GameObject>());
        }
    }
}
