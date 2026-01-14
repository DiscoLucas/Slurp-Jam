using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanThrow : MonoBehaviour
{
    private float timer = 2f;
    public int Damage;
    public float travelSpeed;
    private float hitStrenghThreshold = 1f;
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
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && rb.linearVelocity.magnitude > hitStrenghThreshold)
        {
            EnemytClass enemy = collision.gameObject.GetComponent<EnemytClass>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(Damage);
            }

            Destroy(GetComponentInParent<GameObject>());
        }
    }
}
