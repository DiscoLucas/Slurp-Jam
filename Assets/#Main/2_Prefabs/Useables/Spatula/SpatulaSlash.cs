using System.Linq;
using Unity.InferenceEngine;
using UnityEngine;

public class SpatulaSlash : MonoBehaviour
{
    private float timer = 0.1f;
    public int Damage;
    GameObject[] hitEnemies;

    // Update is called once per frame
    void Update()
    {
        timer-=Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" &! hitEnemies.Contains<GameObject>(collision.gameObject))
        {
            hitEnemies.Append<GameObject>(collision.gameObject);
            EnemytClass Enemy = collision.gameObject.GetComponent<EnemytClass>();
            if(Enemy !=null)
            {
                Debug.Log("hit an enemy!");
                Enemy.EnemyTakeDamage(Damage);
            }
        }
    }
}
