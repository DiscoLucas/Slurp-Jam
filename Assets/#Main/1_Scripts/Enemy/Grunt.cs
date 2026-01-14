using UnityEngine;

public class Grunt : EnemytClass
{
    //public GameObject enemyGoal; // Reference to the enemy goal object
    public Material gruntMaterial1;
    public Material gruntMaterial2;

    private void Start()
    {
        //enemyGoal = GameObject.FindWithTag("EnemyGoal");
    }

    //select random material for grunt when instansiated
    public void SelectRandomMaterialForGunt()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (Random.value > 0.5f)
        {
            renderer.material = gruntMaterial1;
        }
        else
        {
            renderer.material = gruntMaterial2;
        }
    }

    private void Update()
    {
        EnemyMoveTowardsTarget();
    }

    private void MoveTowardsGoal()
    {
        if (enemyGoal != null)
        {
            // Move the grunt towards the enemy goal
            transform.position = Vector3.MoveTowards(transform.position, enemyGoal.transform.position, enemySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyGoal"))
        {
            // Handle reaching the enemy goal (e.g., deal damage, destroy grunt, etc.)
            Debug.Log("Grunt has reached the enemy goal!");
            Debug.Log(enemyName + " has delt " + enemyDamage + " damage to the base");
        }
    }


}
