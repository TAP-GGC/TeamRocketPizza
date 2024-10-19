using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
private IDSTower IDSTower;
    public GameObject exploPref;
    private Dictionary<GameObject, float> enemyCooldowns = new Dictionary<GameObject, float>();
    public float hitCooldown = 0.5f; // Cooldown duration in seconds

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Check if the enemy is in the cooldown period
            if (enemyCooldowns.ContainsKey(other) && Time.time < enemyCooldowns[other])
            {
                return; // Skip if still in cooldown
            }

            // Get the Virus component from the collided enemy
            Virus virus = other.GetComponent<Virus>();
            if (virus != null)
            {
                // Apply damage to the enemy
                virus.TakeDamage(IDSTower.laserDamage);

                // Instantiate the explosion effect at the enemy's position
                GameObject ex = Instantiate(exploPref, virus.transform.position, Quaternion.identity);
                Destroy(ex, 1f); // Destroy the explosion effect after 1 second

                // Set the cooldown time for this enemy
                enemyCooldowns[other] = Time.time + hitCooldown;
            }

            Debug.Log("Enemy found and processed");
        }
    }

    void Start()
    {
        IDSTower = GetComponentInParent<IDSTower>();
    }

    void Update()
    {
        // Optionally, clean up the dictionary by removing entries for enemies that no longer exist
        List<GameObject> keysToRemove = new List<GameObject>();
        foreach (var entry in enemyCooldowns)
        {
            if (entry.Key == null)
            {
                keysToRemove.Add(entry.Key);
            }
        }
        foreach (var key in keysToRemove)
        {
            enemyCooldowns.Remove(key);
        }
    }
}
