using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyGoal"))
        {
            Debug.Log("Arrow hit for " + damage + " damage to " + GameObject.FindGameObjectWithTag("EnemyGoal") );

            // Apply damage to base/goal
            // other.GetComponent<BaseHealth>()?.TakeDamage(damage);

            Destroy(gameObject);
        } else if (other.CompareTag("Player"))
        {
            Debug.Log("Arrow hit for " + damage + " damage to " + GameObject.FindGameObjectWithTag("Player"));
            PlayerContainer player = other.GetComponent<PlayerContainer>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // Apply damage to base/goal
            // other.GetComponent<BaseHealth>()?.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
