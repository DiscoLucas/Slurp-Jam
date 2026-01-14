using System.Collections.Generic;
using UnityEngine;

public class SpatulaSlash : MonoBehaviour
{
    private float timer = 0.1f;
    public int Damage;

    private List<GameObject> hitEnemies = new List<GameObject>();

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
        if (collision.gameObject.CompareTag("Enemy") &&
            !hitEnemies.Contains(collision.gameObject))
        {
            hitEnemies.Add(collision.gameObject);

            EnemytClass enemy = collision.gameObject.GetComponent<EnemytClass>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(Damage);
                Debug.Log("enemy has taken damage");
            }
        }
    }
}
