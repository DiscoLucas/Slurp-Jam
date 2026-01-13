using UnityEngine;

public class Grunt : EnemytClass
{
    public GameObject enemyGoal; // Reference to the enemy goal object

    private void Start()
    {
        enemyGoal = GameObject.FindWithTag("EnemyGoal");
    }

    private void Update()
    {
        MoveTowardsGoal();
    }

    private void MoveTowardsGoal()
    {
        if (enemyGoal != null)
        {
            // Move the grunt towards the enemy goal
            transform.position = Vector3.MoveTowards(transform.position, enemyGoal.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyGoal"))
        {
            // Handle reaching the enemy goal (e.g., deal damage, destroy grunt, etc.)
            Debug.Log("Grunt has reached the enemy goal!");
            Debug.Log(enemyName + " has delt " + damage + " damage to the base");
        }
    }


}
